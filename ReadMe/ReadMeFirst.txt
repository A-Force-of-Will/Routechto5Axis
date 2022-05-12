This template is a standard C# class library project for use with Mastercam.

This project has a reference to NETHook3_0.dll and optionally ToolNetApi.dll and SimAccessManaged.dll located in the root 
directory of your Mastercam installation. However, if you installed into a directory other than the default installation 
directory you may need to update the project references in order for your project to compile. 

This project will work with other versions of Mastercam just remove the references and add the ones from the version of
Mastercam you want to target.

The included Project Setup documents details setup for building debugging a NET-Hook.

**NOTE: Do not use spaces when naming your project as this will cause issues with loading resources
**NOTE: This NET-Hook class library targets the .NET 4.6.1 Framework x64 build.


Visual Studio 2019 Community Edition (Free) is recommended.
https://www.visualstudio.com/en-us/downloads/download-visual-studio-vs.aspx


** POST BUILD STEP **

copy "$(TargetPath)" "$mastercamfolder$chooks\$(TargetFileName)"
copy "$(ProjectDir)Resources\FunctionTable\$(TargetName).ft" "$mastercamfolder$chooks\$(TargetName).ft"

** DEBUGGING **

Start External Application -> $mastercamfolder$Mastercam.exe

Working Directory -> $mastercamfolder$

**NOTE: Edit paths above as needed to point to the installation of Mastercam you are targeting.