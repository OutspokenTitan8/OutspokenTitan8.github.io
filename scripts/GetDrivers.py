import fastf1
import json
import os

output_path = os.path.join("C:/Users/Wills Weaver/Documents/GitHub/OutspokenTitan8.github.io", "drivers.json")
year = 2025
schedule = fastf1.get_event_schedule(year)

# Start from the latest event and search backwards for the first qualifying session with results
for idx in range(len(schedule) - 1, -1, -1):
    event = schedule.iloc[idx]
    try:
        session = fastf1.get_session(year, event['EventName'], 'Q')
        session.load()
        if hasattr(session, 'results') and session.results is not None and not session.results.empty:
            drivers = sorted(set(session.results['Abbreviation']))
            print(f"Drivers found for {event['EventName']} Qualifying: {drivers}")
            with open(output_path, 'w') as f:
                json.dump(drivers, f)
            break
    except Exception as e:
        print(f"Skipping {event['EventName']} due to error: {e}")
else:
    print("No qualifying results found for any event in the season. drivers.json will be empty.")
    with open(output_path, 'w') as f:
        json.dump([], f)
