# Snapchat Memories Date Correction Utility

## Overview

This utility is designed to correct the file dates of Snapchat memories extracted from a backup. Snapchat memory file names often include the correct date, but the file properties ("Created At" and "Modified At") may not reflect this. This program:

1. Extracts the correct date from the file name.
2. Sets the extracted date as the file's creation and modification date.
3. Renames the files by:
   - Removing the date and the underscore (`_`) from the file name.
   - Replacing occurrences of `main` with `snapmem` in the file name.
4. Copies and organizes the files into two separate directories:
   - A `_formatted` directory for processed files.
   - A `_discarded` directory for ignored files.

## Features

- Automatically adjusts file dates using the date embedded in the file name.
- Excludes files with unwanted patterns (`metadata`, `overlay~Snapchat`, and files ending with `overlay`) from processing.
- Copies excluded files to a `_discarded` directory for reference.
- Provides clear console output showing the program's progress.

## How to Use

### Prerequisites
- Visual Studio 2022
- .NET Console Application targeting .NET Framework with support for C# 7.3.

### Steps
1. Clone or download the repository.
2. Open the project in Visual Studio.
3. Build and run the application.
4. Enter the path to the directory containing your Snapchat memories when prompted.
5. The program will:
   - Process the files in the directory.
   - Save formatted files in a new directory named `<original-directory-name>_formatted`.
   - Save ignored files in a directory named `<original-directory-name>_discarded`.
6. At the end of processing, the console will prompt you to close it.

## Tutorial to Extract Snapchat Memories

1. Go to [snapchat.com](https://www.snapchat.com) on your PC.
2. Log in to your Snapchat account.
3. Navigate to **Account Settings** (top left corner).
4. Select **My Data**.
5. Under **Select Data to Include**, check **Export your Memories, Chat Media, and Shared Stories** while keeping all other checkboxes checked.
6. Set a date range or uncheck the date range to select all data.
7. Choose an export option (e.g., slices).
8. Wait for Snapchat to send a download link to your email (may take a few hours).
9. Download and unzip all files.
10. Combine all files into a single directory per type:
    - Place all **memories** in a single directory.
    - Place all **shared stories** in another directory, etc.
11. Open this program and enter the directory path containing the images/videos you want to adjust the dates for.

The program will:
- Read the date from the file name.
- Set the date (12:00:00 fixed time) for each file.
- Rename files properly and copy them to another directory located in the original directory.
- Create a separate directory for discarded files (e.g., files with `metadata`, `overlay~Snapchat`, or ending with `overlay`).

12. Upload your adjusted files to any cloud storage or keep them on your PC.

## Example

### Input Directory:
```
  2016-07-31_b~EiQSFW1rTWFTUkVZMHMyRmxGalowYTYzQRoAGgAyAXxIAlAEYAE-main.mp4
  b~kdady78adyawdUWAadDNwhadHDWUDKwjoiawhduanklawD-main.jpg
  2017-08-12_overlay~SnapchatdjkWADJawkdkawdjkdabnjkadjknajkwdn.png
  2015-09-07_metadata~zip-54061584-8a42-4ada-b960-5b784bc1a26c
```

### Output:

#### Formatted Directory (`SnapchatMemories_formatted`):
```
b~EiQSFW1rTWFTUkVZMHMyRmxGalowYTYzQRoAGgAyAXxIAlAEYAE-snapmem.mp4
```

#### Discarded Directory (`SnapchatMemories_discarded`):
```
2017-08-12_overlay~SnapchatdjkWADJawkdkawdjkdabnjkadjknajkwdn.png
2015-09-07_metadata~zip-54061584-8a42-4ada-b960-5b784bc1a26c
```

## Console Output Example
```
Enter the directory path:
> C:\Users\Roberto\Desktop\SnapchatMemories\memories

Fetching files...
Found 4 files. Processing...
Skipping file: 2017-08-12_overlay~SnapchatdjkWADJawkdkawdjkdabnjkadjknajkwdn.png (1/3)
Copying file: 2017-08-12_overlay~SnapchatdjkWADJawkdkawdjkdabnjkadjknajkwdn.png (1/3)
Copied file: 2017-08-12_overlay~Snapchat (1/4)
Skipping file: 2015-09-07_metadata~zip-54061584-8a42-4ada-b960-5b784bc1a26c (2/3)
Copying file: 2015-09-07_metadata~zip-54061584-8a42-4ada-b960-5b784bc1a26c (2/3)
Copied file: 2015-09-07_metadata~zip-54061584-8a42-4ada-b960-5b784bc1a26c (2/3)
Skipping file: b~kdady78adyawdUWAadDNwhadHDWUDKwjoiawhduanklawD-main.jpg (no valid date found)
Processing file: 2016-07-31_b~EiQSFW1rTWFTUkVZMHMyRmxGalowYTYzQRoAGgAyAXxIAlAEYAE (3/3)
Processed file: 2016-07-31_b~EiQSFW1rTWFTUkVZMHMyRmxGalowYTYzQRoAGgAyAXxIAlAEYAE (3/3)

Processing complete. All files saved in the respective directories.
Press 'Y' to close the console, or any other key to keep it open.
```

## Notes

- Files without a valid date in the name will be skipped and logged in the console.
- Ensure the source directory path is valid and accessible.

## License

This project is open-source and available under the MIT License. Feel free to contribute or suggest improvements!

