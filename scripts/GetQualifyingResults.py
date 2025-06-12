import fastf1
import pandas as pd
import os
import json
import tkinter as tk
from tkinter import filedialog
import subprocess
import platform
import time
import sys
from datetime import datetime, timezone
import traceback

LOG_FILE = "qualifying_error.log"

TEAM_COLORS = {
    "Red Bull Racing": "#1E5BC6",
    "Ferrari": "#E8002D",
    "Mercedes": "#00D2BE",
    "McLaren": "#FF8700",
    "Aston Martin": "#229971",
    "Alpine F1 Team": "#2293D1",
    "Williams": "#37BEDD",
    "AlphaTauri": "#2B4562",
    "RB": "#6692FF",
    "Alfa Romeo": "#900000",
    "Sauber": "#52E252",
    "Haas F1 Team": "#B6BABD",
    "Unknown": "#AAAAAA"
}

def log_error(msg):
    with open(LOG_FILE, "a", encoding="utf-8") as f:
        f.write(f"[{datetime.now().isoformat()}] {msg}\n")

def format_lap_time(value):
    if pd.isna(value):
        return "—"
    try:
        td = pd.to_timedelta(value)
        total_ms = td.total_seconds() * 1000
        minutes = int(total_ms // 60000)
        seconds = (total_ms % 60000) / 1000
        return f"{minutes}:{seconds:06.3f}"
    except Exception as e:
        log_error(f"Error formatting lap time '{value}': {e}")
        return str(value)

def get_repo_path():
    config_path = os.path.expanduser("~/.f1_dashboard_config.json")
    if os.path.exists(config_path):
        try:
            with open(config_path, "r") as f:
                return json.load(f).get("repo_path")
        except Exception as e:
            log_error(f"Error reading config file: {e}")

    print(" Select your GitHub repo folder (where index.html should go)...", file=sys.stderr)
    root = tk.Tk()
    root.withdraw()
    folder = filedialog.askdirectory(title="Select GitHub Repo Folder")
    if not folder:
        print(" No folder selected. Exiting.", file=sys.stderr)
        sys.exit(1)
    try:
        with open(config_path, "w") as f:
            json.dump({"repo_path": folder}, f)
    except Exception as e:
        log_error(f"Error writing config file: {e}")
    return folder

def create_wrapper_html(folder_path, filename="qualifying_results.html", refresh_seconds=60):
    wrapper_html = f"""<!DOCTYPE html>
<html>
<head>
  <meta http-equiv="refresh" content="{refresh_seconds}">
  <title>Qualifying Wrapper</title>
  <style>
    html, body, iframe {{
      margin: 0; padding: 0; height: 100%; width: 100%; border: none;
    }}
    iframe {{
      display: block;
      border: none;
    }}
  </style>
</head>
<body>
  <iframe src="{filename}" width="100%" height="100%" frameborder="0"></iframe>
</body>
</html>"""
    wrapper_path = os.path.join(folder_path, "wrapper_qualifying_results.html")
    with open(wrapper_path, 'w', encoding='utf-8') as f:
        f.write(wrapper_html)
    print(f"Wrapper HTML created at {wrapper_path}")

def main():
    try:
        os.makedirs("cache", exist_ok=True)
        fastf1.Cache.enable_cache("cache")

        schedule = fastf1.get_event_schedule(2025)
        # Fix timezone if needed
        if pd.api.types.is_datetime64_any_dtype(schedule['Session1Date']):
            if schedule['Session1Date'].dt.tz is None:
                schedule['Session1Date'] = schedule['Session1Date'].dt.tz_localize("UTC")
            else:
                schedule['Session1Date'] = schedule['Session1Date'].dt.tz_convert("UTC")

        now = datetime.now(timezone.utc)
        past_events = schedule[schedule['Session1Date'] < now]

        if past_events.empty:
            msg = "No past sessions found."
            print(msg, file=sys.stderr)
            log_error(msg)
            time.sleep(10)
            sys.exit(1)

        latest_qualifying = None
        for _, event in past_events[::-1].iterrows():
            try:
                session = fastf1.get_session(2025, int(event['RoundNumber']), 'Q')
                session.load()
                if session.results is not None and not session.results.empty:
                    latest_qualifying = event
                    break
            except Exception as e:
                log_error(f"Skipping event {event['EventName']} due to error: {e}")

        if latest_qualifying is None:
            msg = "No completed qualifying sessions found."
            print(msg, file=sys.stderr)
            log_error(msg)
            time.sleep(10)
            sys.exit(1)

        gp_name = latest_qualifying["EventName"]
        gp_round = int(latest_qualifying["RoundNumber"])
        session = fastf1.get_session(2025, gp_round, "Q")
        session.load()

        print(f"Using qualifying data from Round {gp_round}: {gp_name}", file=sys.stderr)

        results = []
        for _, row in session.results.iterrows():
            abbrev = row["Abbreviation"]
            pos = row["Position"]
            q1 = format_lap_time(row["Q1"])
            q2 = format_lap_time(row["Q2"])
            q3 = format_lap_time(row["Q3"])

            try:
                driver_info = session.get_driver(abbrev)
                name = driver_info.get("FullName") or f"{driver_info['GivenName']} {driver_info['FamilyName']}"
                team = driver_info.get("TeamName", "Unknown")
            except Exception:
                name = abbrev
                team = "Unknown"

            elim_status = "Q3"
            if q3 == "—":
                elim_status = "Q2"
            if q2 == "—":
                elim_status = "Q1"

            color = TEAM_COLORS.get(team, "#AAAAAA")

            results.append({
                "Pos": pos,
                "Driver": name,
                "Team": team,
                "Q3": q3,
                "Q2": q2,
                "Q1": q1,
                "TeamColor": color,
                "Elim": elim_status
            })

        df = pd.DataFrame(results)
        df["Pos"] = df["Pos"].apply(lambda x: str(int(x)) if pd.notna(x) else "—")
        df["PosNum"] = pd.to_numeric(df["Pos"], errors="coerce")
        df = df.sort_values(by="PosNum")
        df = df.drop(columns=["PosNum"])

        html_rows = ""
        for _, row in df.iterrows():
            bg_color = {
                "Q1": "#ffcccc",
                "Q2": "#fff5b1",
                "Q3": "#ffffff"
            }.get(row["Elim"], "#ffffff")

            html_rows += f"""
            <tr style="border-left: 5px solid {row['TeamColor']}; background-color: {bg_color};">
                <td>{row['Pos']}</td>
                <td style="color: {row['TeamColor']};">{row['Driver']}</td>
                <td style="color: {row['TeamColor']};">{row['Team']}</td>
                <td>{row['Q3']}</td>
                <td>{row['Q2']}</td>
                <td>{row['Q1']}</td>
            </tr>
            """

        html_template = f"""<!DOCTYPE html>
<html lang="en">
<head>
<meta charset="UTF-8" />
<title>{gp_name} – Qualifying Results</title>
<style>
    body {{
        font-family: Arial, sans-serif;
        padding: 10px;
        margin: 0;
        background: #ffffff;
        color: #222;
    }}
    h1 {{
        text-align: center;
        font-size: 1.3em;
        margin-bottom: 20px;
    }}
    .table-container {{
        overflow-x: auto;
        -webkit-overflow-scrolling: touch;
        width: 100%;
    }}
    table {{
        border-collapse: collapse;
        width: 100%;
        min-width: 500px;
    }}
    th, td {{
        border: 1px solid #ccc;
        padding: 6px 10px;
        text-align: center;
        font-size: 0.9em;
        white-space: nowrap;
    }}
    th {{
        background-color: #222;
        color: white;
        position: sticky;
        top: 0;
        z-index: 1;
    }}
    @media (max-width: 480px) {{
        th, td {{
            padding: 5px 6px;
            font-size: 0.75em;
        }}
        h1 {{
            font-size: 1em;
        }}
    }}
</style>
</head>
<body>
    <h1>{gp_name} – Qualifying Results</h1>
    <div class="table-container">
        <table>
            <thead>
                <tr>
                    <th style="border-left: 5px solid #222;">Pos</th>
                    <th>Driver</th>
                    <th>Team</th>
                    <th>Q3</th>
                    <th>Q2</th>
                    <th>Q1</th>
                </tr>
            </thead>
            <tbody>
                {html_rows}
            </tbody>
        </table>
    </div>
</body>
</html>"""

        repo_path = get_repo_path()
        filename = os.path.join(repo_path, "qualifying_results.html")

        if os.path.exists(filename):
            print(f"{filename} already exists — overwriting.", file=sys.stderr)

        with open(filename, "w", encoding="utf-8") as f:
            f.write(html_template)

        print(f"{filename} saved.", file=sys.stderr)

        # Create the wrapper HTML file that refreshes every 60 seconds
        create_wrapper_html(repo_path, filename="qualifying_results.html", refresh_seconds=60)

    except Exception as e:
        log_error(f"Unexpected error in main(): {traceback.format_exc()}")
        print("An error occurred. Check qualifying_error.log for details.", file=sys.stderr)
        time.sleep(10)
        sys.exit(1)

if __name__ == "__main__":
    main()
