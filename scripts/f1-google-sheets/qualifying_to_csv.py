import fastf1
import pandas as pd
import os

# Ensure cache directory exists
os.makedirs('cache', exist_ok=True)
fastf1.Cache.enable_cache('cache')

# Get the 2025 season schedule
schedule = fastf1.get_event_schedule(2025)

# Try to find the most recent qualifying session with results
latest_qualifying = None
for _, event in schedule[::-1].iterrows():  # Go backwards through the calendar
    try:
        session = fastf1.get_session(2025, int(event['RoundNumber']), 'Q')
        session.load()
        if session.results is not None and not session.results.empty:
            latest_qualifying = event
            break
    except Exception:
        continue

# No qualifying sessions found
if latest_qualifying is None:
    raise ValueError("❌ No completed qualifying sessions found for 2025.")

# Info about latest session
gp_name = latest_qualifying['EventName']
gp_round = int(latest_qualifying['RoundNumber'])
print(f"📍 Fetching latest qualifying: Round {gp_round} – {gp_name}")

# Reload session in case it wasn't passed from loop
session = fastf1.get_session(2025, gp_round, 'Q')
session.load()

# Prepare results list
results = []

for drv in session.drivers:
    laps = session.laps.pick_drivers(drv)
    best = laps.pick_fastest()
    driver_info = session.get_driver(drv)

    # Driver name and team
    try:
        name = driver_info['FullName']
    except KeyError:
        name = f"{driver_info['GivenName']} {driver_info['FamilyName']}"
    team = driver_info['TeamName']

    # Get Q1/Q2/Q3 times
    result = session.results[session.results['Abbreviation'] == drv]
    q1 = str(result['Q1'].values[0]) if 'Q1' in result and pd.notna(result['Q1'].values[0]) else "—"
    q2 = str(result['Q2'].values[0]) if 'Q2' in result and pd.notna(result['Q2'].values[0]) else "—"
    q3 = str(result['Q3'].values[0]) if 'Q3' in result and pd.notna(result['Q3'].values[0]) else "—"

    if best is not None and pd.notna(best['Position']) and pd.notna(best['LapTime']):
        results.append({
            'Pos': best['Position'],
            'Driver': name,
            'Team': team,
            'Best Lap': str(best['LapTime']),
            'Q1': q1,
            'Q2': q2,
            'Q3': q3
        })
    else:
        results.append({
            'Pos': None,
            'Driver': name,
            'Team': team,
            'Best Lap': "No Time",
            'Q1': q1,
            'Q2': q2,
            'Q3': q3
        })

# Convert to DataFrame
df = pd.DataFrame(results)

# Separate valid and invalid results
valid_df = df.dropna(subset=['Pos'])
valid_df['Pos'] = valid_df['Pos'].astype(int)
valid_df = valid_df.sort_values(by='Pos')

no_time_df = df[df['Pos'].isna()]
no_time_df['Pos'] = "—"

# Combine and export
final_df = pd.concat([valid_df, no_time_df], ignore_index=True)
filename = f"qualifying_results_{gp_name.replace(' ', '_')}.csv"
final_df.to_csv(filename, index=False)

print(f"✅ Qualifying results saved to {filename}")
