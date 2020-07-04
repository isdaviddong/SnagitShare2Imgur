# SnagitShare2Imgur Project Status
[![Build status](https://twlab.visualstudio.com/SnagitShare2Imgur/_apis/build/status/SnagitShare2Imgur-ASP.NET%20Core-CI)](https://twlab.visualstudio.com/SnagitShare2Imgur/_build/latest?definitionId=40) ![Nuget](https://img.shields.io/nuget/v/SnagitShare2Imgur)
![Nuget](https://img.shields.io/nuget/dt/SnagitShare2Imgur)

Upload image to Imgur when Snagit finish Capture  
===
Snagit is a super convenient screenshot tool, you can easily take screenshots through the HotKey you selected, and you can automatically upload the captured pictures to many locations.

But among these preset locations, is not include the Imgur.com that I usually used. Imgur.com is a free picture storage, and there is APIs to use, which is quite convenient.

As a result, I can only save the picture first, and then upload it to Imgur through the webpage, it is very troublesome.

Fortunately, Snagit allows you to process the image files after screenshots through specific programs. As a programmer, the advantage of writing programs is that you can solve your own problems. I started to write a small program using .net core, which can be automatically uploaded to Imgur after Snagit screenshots. 

Usage:
===

Please install dotnet core runtime first. If the installation is successful, you will see the version number by executing 
dotnet --version 
in the command line. 

Please install version 3.1.100 or higher. Then please execute the following command in the console mode:

dotnet tool install --global SnagitShare2Imgur

![](https://i.imgur.com/SUYlt27.png)
This means that the tool has been successfully installed on your computer.
