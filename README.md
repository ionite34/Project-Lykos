# Project-Lykos
 
[![.NET Desktop Tests](https://github.com/ionite34/Project-Lykos/actions/workflows/dotnet-desktop-tests.yml/badge.svg)](https://github.com/ionite34/Project-Lykos/actions/workflows/dotnet-desktop-tests.yml)

This project has been split up to include a `Master` and a `Worker` or slave process. In the original app there was a `Queue` of all the `ProcessTask` objects that needed to be processed.
What happens now is that the _Master_ app can generate the `ProcessTask` objects, and then it stored those objects in a database, this then allows for distributed processing of those objects.

The _Worker_ process reads a batch of the records from the database that haven't been processed yet, it then re-creates the `ProcessTask` object from the details in the database, then it continues to process those records.

The concept of _Multiple Threads_ is different in this implementation, to get multiple _threads_ you need to run multiple worker processors, but you will need to run them on mutliple machines.
You can't easily run multiple workers on the same machine as it uses the same temp storage space but also the Face FX library doesn't really allow this to work anyway

> In writing this, I just realised that even if you choose not to use the worker process for distributed processing, we could make a minor change to the original
> interface to log to the database at all the same points, that would allow you to more easily sort through the logs instead of going through mutliple files.

## Entity Framework Core
The database context used in this app is Entity Framework Core. It will also need access to an MS SQL Database, you can use SQL Server Express if you do not have access to a running instance, [download SQL Server Express 2019 here](https://www.microsoft.com/en-us/Download/details.aspx?id=101064)
You should also install the SSMS - SQL Server Management Studio, that is an option in the SQL Express installer, but can be found here too: https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15

## Core
To allow for code re-use some of the logic from the original app was moved to the `Core` project. 

This is also where the data-schema is defined, you will find that in the `Data` folder. `LykosQueueContext` is the name of the Entity Framework DbContext.

## Master
This program is a clone of the original processor UI, the difference is that this U will only pre-process the files and add entries into a database queue.
For this application to work you will need the connection string to a server, the database does not need to exist and the code will create the database on the first run.

Update the connection string in the appsettings.json:

      "ConnectionStrings": {
        "LykosQueue": "Server=localhost\\SQLEXPRESS;Database=LykosQueue;Trusted_Connection=True;"
      }

## Worker
The worker process is a Windows Service, it is continuosly running and will poll the database for records in the `QueueEntries` table.
The expectation is that you can deploy this service to multiple machines, and they will talk to the database, the other caveat is that the path references for the records stored in the database **MUST** be valid in all the worker environments.
Either use full UNC paths or make sure that the network shares are consistent on all worker stations.

You can run the executable manually, or can publish and run as a service, please read through these articles for guidance:

- [Publish the App](https://docs.microsoft.com/en-us/dotnet/core/extensions/windows-service#publish-the-app)
- [Install as Windows Service](https://docs.microsoft.com/en-us/dotnet/core/extensions/windows-service#create-the-windows-service)

By default, the current code is set to abort the windows service for critical errors such as:

- Fonix data not found
- Temp folders cannot be cleared (usually because they are in use)
- Can't find or start the external process

### Checking Logs
No UI has been created for this yet, but you can inspect the database directly for logs in 2 places:

> **NOTE:** The database is used here instead of files, querying the database makes it a lot easier to lookup progress and analyse average runtimes and stuff like that ;)

1. **LogEntries**  
   These are general runtime events view the latest to get a feel for if the process is still running, the following SQL query will get you the 20 most recent entries

         SELECT TOP (20) *
         FROM [LykosQueue].[dbo].[LogEntries]
         ORDER BY Id DESC

   
   |Id	|EntryDate	                        |MachineName	    |Process	        |Severity	|Message|
   |---|-----------------------------------|-------------------|-------------------|-----------|--------|
   |28	|2022-04-15 23:29:27.4490819 +10:00	|DESKTOP-IH6OBS6	|Batch	            |40	        |Unable to start sub process|
   |27	|2022-04-15 23:28:56.0373252 +10:00	|DESKTOP-IH6OBS6	|GetNextBatch	    |80	        |Dequeued 1 records|
   |26	|2022-04-15 23:25:41.9766436 +10:00	|DESKTOP-IH6OBS6	|ExternalProcess	|40	        |Fonix data not found|

2. **QueueEntries**
   These are the actual queued tasks, its pretty straight forward, `Processed` represents a succesful and completed process.

		SELECT TOP (100) *
		FROM [LykosQueue].[dbo].[QueueEntries]
		WHERE Processed IS NULL
		ORDER BY ID ASC

   |Id	|Enqueued							|Dequeued							|LastDequeued						|Processed	|ErrorCount	|ErrorMessage	|WavSourcePath	|LipOutputPath	|Text	|UseDllDirect	|UseNativeResampler	|WavTempPath	|UUID									|Command	|Runtime	|ExitCode	|Output|
   |--|-----------------------------------|-----------------------------------|------------------------------------|----------|------------|-------------|---------------|--------------|--------|---------|--------------------|---------------------|--------------------------------------|-----------|---------|-------------|--------|
   | 3	|2022-04-15 19:57:04.1580234 +10:00	|2022-04-16 00:53:31.9595531 +10:00	|2022-04-16 00:53:31.9595531 +10:00	|NULL		|1			|NULL			|NULL			|NULL			|NULL	|NULL			|NULL				|NULL			|B92CEEE5-DADD-4E32-A07A-2A764BA7BA25	|NULL		|NULL		|NULL		|NULL|
   |4	|2022-04-15 19:57:06.8918375 +10:00	|NULL								|NULL								|NULL		|NULL		|NULL			|NULL			|NULL			|NULL	|NULL			|NULL				|NULL			|6B00FE97-0701-439B-AC25-23E22C51C80F	|NULL		|NULL		|NULL		|NULL|
   |5	|2022-04-15 19:57:07.0428221 +10:00	|NULL								|NULL								|NULL		|NULL		|NULL			|NULL			|NULL			|NULL	|NULL			|NULL				|NULL			|54761B9B-9D78-4AD0-AD9D-39EF38A9EA76	|NULL		|NULL		|NULL		|NULL|

   Some of these fields are for the general _Queue_ management, you should recognise the fields that are the input properties for the `ProcessTask` class as well as the outputs.
   If you look at the `~\ionite34\Project-Lykos\Project Lykos Core\Data\QueueEntry.cs` file you will see the legacy log file format of this class, that should piece together the field meanings:


        public string FormatLogText()
        {
            var log = new StringBuilder();
            log.AppendLine("==========================================================");
            log.AppendLine($"Enqueued: {Enqueued}");
            log.AppendLine($"Id: [{Id}] {UUID}");
            log.AppendLine($"Dequeued: {LastDequeued}");
            log.AppendLine($"Processed: {Processed}");
            log.AppendLine("==========================================================");
            log.AppendLine($"Processing mode: {(UseDllDirect.GetValueOrDefault() ? "DLL Direct" : "External Process")}");
            log.AppendLine($"Resampling mode: {(UseNativeResampler.GetValueOrDefault() ? "Native" : "Custom")}");
            log.AppendLine($"Path to Audio: {WavSourcePath}");
            log.AppendLine($"Path to Resampled Audio: {WavTempPath}");
            log.AppendLine($"Path to Output: {LipOutputPath}");
            log.AppendLine($"Audio File Name: {(String.IsNullOrWhiteSpace(WavSourcePath) ? String.Empty : Path.GetFileName(WavSourcePath))}");
            log.AppendLine($"Text: {Text}");
            log.AppendLine("==========================================================");
            log.AppendLine($"Exit Code: {ExitCode}");
            log.AppendLine($"Run Time: {Runtime}");
            log.AppendLine("==========================================================");
            log.AppendLine("FaceFX output:");
            log.AppendLine(Output ?? String.Empty);
            log.AppendLine("==========================================================");
            log.AppendLine($"Error Count: {Processed}");
            log.AppendLine(ErrorMessage ?? string.Empty);
            log.AppendLine("==========================================================");
            return log.ToString();
        }


## Manually Inserting Tasks
You can write directly from your python processing to the database, the following SQL shows the basic elements:

	DECLARE @WavSourcePath varchar(max) = '';
	DECLARE @LipOutputPath varchar(max) = '';
	DECLARE @Text varchar(max) = '';
	DECLARE @UseNativeResampler BIT = 0;

	INSERT INTO [LykosQueue]..[QueueEntries] (Enqueued, UUID, WavSourcePath, LipOutputPath, Text, UseNativeResampler) 
	VALUES (SYSDATETIMEOFFSET(), CAST(NewID() as varchar(40)), @WavSourcePath, @LipOutputPath, @Text, @UseNativeResampler)


# NEXT STEPS
> I am not able to really test this myself at this stage, I would need some real world files to work with and I am stuck with this error:
>
>    [FaceFX 04]: Log callback registered.
>    [FXE] > Init OK
>    [FXE] > RFI
>    A CreationKit parent process ID (0x0) was supplied, but wasn't able to be queried (87).
>
> I haven't figured out how to trap that output message and store it in the database, it is currently logged as `Unable to start sub process`.

