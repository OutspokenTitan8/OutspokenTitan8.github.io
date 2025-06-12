import json
import pandas as pd

def json_to_html(input_json: str, output_html: str):
    # Load the JSON data
    with open(input_json, 'r', encoding='utf-8') as f:
        data = json.load(f)

    # Convert to DataFrame
    df = pd.DataFrame(data)

    # Keep only the desired columns (adjust column names as needed)
    columns_to_keep = ['PlayerName', 'Pick10th', 'PickDNF']
    df = df[columns_to_keep]

    # Create HTML table from DataFrame
    html_table = df.to_html(index=False, classes='table table-striped', border=0)

    # Build full HTML page
    html_page = f"""<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>Guesses Export</title>
    <style>
        body {{ font-family: Arial, sans-serif; background-color: #1a1a1a; color: #f0f0f0; padding: 20px; }}
        h2 {{ color: #00ccff; }}
        table {{ width: 100%; border-collapse: collapse; background-color: #2a2a2a; }}
        th, td {{ padding: 10px; border: 1px solid #444; }}
        th {{ background-color: #333; color: #fff; }}
        tr:nth-child(even) {{ background-color: #1e1e1e; }}
    </style>
</head>
<body>
    <h2>F1 Guess Grid Export</h2>
    {html_table}
</body>
</html>
"""

    # Write to HTML file
    with open(output_html, 'w', encoding='utf-8') as f:
        f.write(html_page)

    print(f"HTML file created: {output_html}")

# Example usage
if __name__ == "__main__":
    json_to_html(r"C:\Users\Wills Weaver\Documents\GitHub\OutspokenTitan8.github.io\guesses_export.json",
                 r"C:\Users\Wills Weaver\Documents\GitHub\OutspokenTitan8.github.io\guesses_output.html")
