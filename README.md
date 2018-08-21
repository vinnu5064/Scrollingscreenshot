# Scrollingscreenshot
This file contains instructions on how to set up your environment to work with the source code:

Necessary Softwares:

    Microsoft Visual Studio Community Edition(.Net framework 4.6) 
    It is used to develop/debug the source code.

Running the tests:

The source code is ready to build as an Uipath Activity. Ensure that below references have been added to the project<br/>
i) system.Activities<br/>
ii) system.Componentmodel.Composistion

Deployment:

1. Once you are done with cloning the source code, build the solution. The Output panel is displayed, informing you that the file is built and displays its path. In our case the Scrollingscreenshot.dll file is created.<br/>
2. Follow the steps listed here https://activities.uipath.com/docs/creating-a-custom-activity#section-creating-the-nuget-package to create .nupkg file.<br/>
3. Copy the .nupkg file inside the Packages folder of your UiPath Studio install location (%USERPROFILE%.nuget\Packages).The NuGet Package containing your custom activity is now ready to be loaded in UiPath Studio.(Refer-https://activities.uipath.com/docs/creating-a-custom-activity#section-loading-the-nuget-package-in-uipath-studio)

License:

This project is licensed under the MIT License - see the LICENSE.md file for details
