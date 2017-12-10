EFCore is currently bugged so that trying to run Add-Migration
against a project built only against Fall Creators Update causes
exceptions. This project exists solely as a workaround to that.

To run Add-Migration follow these steps:

* Set Thingventory.MigrationHack as the startup project.
* Open Package Manager Console and select Thingventory.Core as the default project.
* Run Add-Migration as normal.
* Set Thingventory back as the startup project and unload MigrationHack
