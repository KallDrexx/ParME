---
title: FlatRedBall Integration
permalink: general_frb.html
---

[FlatRedBall](http://flatredball.com/) is a cross-platform, MonoGame based engine optimized for 2d games development.  Parme has integrations into FlatRedBall to make particle effects extremely easy to add to a FlatRedBall game.

## Integration Steps

The steps to integrate Parme in your FlatRedBall game are:

### Project Infrastructure

Parme's C# libraries utilize `.Net Standard 2.0`, and therefore will only work with the following minimum runtime versions:

* .NET Core 2.0
* .NET Framework 4.6.1
* Mono 5.4
* Xamarin.iOS 10.14
* Xamarin.Mac 3.8
* Xamarin.Android 8.0
* Universal Windows Platform 10.0.16299

Furthermore, if your project uses Desktop (old / non-SDK) style `csproj` projects, then you will need to add the following to the first `PropertyGroup` node in your `csproj` file: `<AutoGenerateBindingRedirects>true</AutoGenerateBindingRedirects>`.

This is required for the `System.Numerics.Vector` nuget package, which Parme relies on.  If this is not done than a `FileLoadException` will occur when it tries to load this library. 

### Nuget Package

Add the latest version of the [Parme.Frb Nuget Package](https://www.nuget.org/packages/Parme.Frb/) to the project.  This should automatically add `Parme.Core`, `Parme.CSharp`, and `Parme.MonoGame` to your project.

It will also add a `/Plugins` folder to your FlatRedBall game.   This folder contains the plugin for Glue to add support for code generation and adding emitters to entities.  Glue will automatically find the plugin in this folder and use it for this project.

**Note:** Since the NuGet package manages the files in this folder, if an upgrade or downgrade of the NuGet package is performed while Glue is open than the NuGet change will fail, as Glue will have the plugin files open and locked.   

### Add Your Emitter Logic Definition Files

Open Glue and add your desired `.emlogic` files to your game's global content.  

When an `.emlogic` file is added to a Glue project, the Parme plugin will automatically generate code in the `/Particles` folder of your project.  The generated file will reflect the C# representation of your emitter's logic, and will be passed into any emitter you create.

### Add an Emitter To An Entity

Go to the entity you want to add an emitter to and right click the `Ojbects` node to select `Add Object`.  Select the `FlatRedBall or Custom Type` and find the `Parme Particle Emitter` entry. 

![Add Parme Emitter Image](images/glue_add_object_dialog.PNG) 

Give the emitter a name and click `Ok`.  You can now click on your emitter and add set some variables for it.

![Emitter Variables Image](images/glue_emitter_vars.png)

The variables that can be set are:

* Emitter Logic (*mandatory*) - this defines which emitter logic definition this emitter will use, and the entries in the drop down will correspond to the `.emlogic` files that have been added to the glue project
* Emitter Grouping - Emitter groups allow grouping emitters together that belong on the same layer.  This allows for some emitters to render in the background, some in the foreground, etc...  If no group specified than a default group will be used.
* Is Emitting - If checked than the emitter will start out emitting when it is created.  If false than the emitter must be manually started in code.
* X/Y Offset - Where the emitter is located in relation to the entity it is attached to.  This is useful if you do not want particles emitted at the entity's origin.

Once these values are to your liking you can run the game and you should see your particles!

![Example Particles In FRB](images/frb_finished_example.gif) 


