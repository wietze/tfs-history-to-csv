# tfs-history-to-csv
## Summary
This simple C# tool gets all Team Foundation Server (TFS) changeset history of a given project and generates a Comma Delimited Values (CSV) file with relevant details. The CSV file can be used for futher analysis.

By default, the following is exported:
* Changeset ID
* ISO Date
* Date
* User
* Comment
* Change (Add/Edit/Delete/Lock etc.)
* File

## Requirements
In order to run the tool, make sure the following is installed:

* .NET Framework 4.5 or higher
* Visual Studio Server with Team Foundation Server (i.e. Visual Studio Professional or better).  
  In particular, the following libraries are required:
    * `Microsoft.TeamFoundation.Client`
    * `Microsoft.TeamFoundation.VersionControl.Client`

## Usage
 0. Download the executable [here](https://github.com/wietze/tfs-history-to-csv/releases).
 1. Run `tfs-history-to-csv.exe`
 2. On the first dialog, select your _TFS server_, _Team Project Collection_ and _Team Project_. Click `Connect`.
 3. On the second dialog, specify an output location for the CSV. Click `Save`.
