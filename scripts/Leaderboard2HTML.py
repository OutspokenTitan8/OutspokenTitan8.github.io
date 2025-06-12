import json
import os
from datetime import datetime

def json_to_html_table(json_file, html_file):
    with open(json_file, 'r', encoding='utf-8') as f:
        data = json.load(f)

    if not data:
        table = "<p>No data to display.</p>"
    else:
        headers = list(data[0].keys())
        # Change PlayerName header to Name
        headers = ['Name' if h == 'PlayerName' else h for h in headers]

        # Move position column ("Pos" or "Position") to front if exists
        pos_col = None
        for candidate in ["Pos", "Position"]:
            if candidate in headers:
                pos_col = candidate
                break

        if pos_col:
            headers.remove(pos_col)
            headers.insert(0, pos_col)

        # Rename "Pos" or "Position" header to "Position"
        headers = ['Position' if h in ['Pos', 'Position'] else h for h in headers]

        table = "<table>\n<tr>"
        for h in headers:
            table += f"<th>{h}</th>"
        table += "</tr>\n"

        for idx, row in enumerate(data):
            # Get position, fallback to idx+1 if invalid
            pos_val = row.get("Pos") or row.get("Position")
            try:
                pos = int(pos_val)
            except (TypeError, ValueError):
                pos = idx + 1

            row_class = ""
            if pos == 1:
                row_class = 'class="gold"'
            elif pos == 2:
                row_class = 'class="silver"'
            elif pos == 3:
                row_class = 'class="bronze"'

            table += f"<tr {row_class}>"
            for h in headers:
                # Map 'Name' back to 'PlayerName' key in data
                key = 'PlayerName' if h == 'Name' else h
                val = row.get(key, "")
                # Bold and center the position column
                if h == 'Position':
                    table += f"<td class='pos'>{val}</td>"
                else:
                    table += f"<td>{val}</td>"
            table += "</tr>\n"
        table += "</table>"

    html = f"""<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <title>Leaderboard</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            padding: 20px;
            background-color: #f9f9f9;
        }}
        table {{
            border-collapse: collapse;
            width: 100%;
            max-width: 800px;
            margin: auto;
            background-color: #fff;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
        }}
        th, td {{
            border: 1px solid #ddd;
            text-align: left;
            padding: 6px 8px;
            white-space: nowrap;
        }}
        th {{
            background-color: #ddd;
            color: black;
            text-align: center;
            font-weight: bold;
        }}
        tr:nth-child(even) {{
            background-color: #f9f9f9;
        }}
        tr:hover {{
            background-color: #f1f1f1;
        }}
        /* Position column */
        td.pos {{
            text-align: center;
            font-weight: bold;
            width: 50px;
        }}
        /* Top 3 highlight colors */
        tr.gold {{
            background-color: #ffd70080; /* gold */
            font-weight: bold;
        }}
        tr.silver {{
            background-color: #c0c0c080; /* silver */
        }}
        tr.bronze {{
            background-color: #cd7f3280; /* bronze */
        }}
    </style>
</head>
<body>
{table}
</body>
</html>"""

    with open(html_file, 'w', encoding='utf-8') as f:
        f.write(html)

def create_wrapper_html(folder_path, refresh_seconds=60):
    # Add timestamp query param to bust cache
    timestamp = int(datetime.utcnow().timestamp())
    iframe_src = f"htmlleaderboard.html?t={timestamp}"

    wrapper_html = f"""<!DOCTYPE html>
<html>
<head>
  <meta http-equiv="refresh" content="{refresh_seconds}">
  <title>Leaderboard Wrapper</title>
  <style>
    html, body, iframe {{
      margin: 0; padding: 0; height: 100%; width: 100%; border: none;
    }}
    iframe {{
      display: block;
      border: none;
    }}
  </style>
  <script>
    // Reload iframe with updated timestamp on each wrapper refresh
    function reloadIframe() {{
      var iframe = document.getElementById('leaderboardFrame');
      var src = iframe.src.split('?')[0];
      iframe.src = src + '?t=' + new Date().getTime();
    }}
    window.onload = function() {{
      setTimeout(reloadIframe, 100);
    }};
  </script>
</head>
<body>
  <iframe id="leaderboardFrame" src="{iframe_src}" width="100%" height="100%" frameborder="0"></iframe>
</body>
</html>"""

    wrapper_path = os.path.join(folder_path, "leaderboard_wrapper.html")
    with open(wrapper_path, 'w', encoding='utf-8') as f:
        f.write(wrapper_html)
    print(f"Wrapper HTML created at {wrapper_path}")

if __name__ == "__main__":
    folder = "C:/Users/Wills Weaver/Documents/GitHub/OutspokenTitan8.github.io"
    json_path = os.path.join(folder, "leaderboard.json")
    html_path = os.path.join(folder, "htmlleaderboard.html")

    json_to_html_table(json_path, html_path)
    create_wrapper_html(folder, refresh_seconds=60)

    print("HTML table and wrapper generation complete.")
