---
title: MonoGame Integration
permalink: general_monogame.html
---

Parme has built-in MonoGame support to make it easy to add it to any MonoGame based engine.  

## Infrastructure Setup

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

Once the project is using the correct runtime, the next thing that has to be done is to add the [Parme.MonoGame Nuget Package](https://www.nuget.org/packages/Parme.MonoGame/) to your project.  

With the Parme library added, your game needs to instantiate several objects:

### Texture File Loader

A custom implementation of the `ITextureFileLoader` interface is needed so emitters use the same content pipelines used by the main game to load the textures.  This will allow the game to cache the textures properly and not reload from disk if particles use the same spritesheet as other objects in the game.

### ParticlePool

Each `ParticlePool` instance represents a group of pooled particle structures that are re-usable by multiple emitters.  When an emitter is created, it is given a `ParticlePool` instance it will reserve slots particles from.  This means the game can create and destroy emitters as needed while reducing garbage collection pressure (since the significant bulk of an emitter's memory is the shared pool that will not be deallocated on collection).

Each game can use as many or little number of `ParticlePool` instances as it wants.  It is recommended that each rendering group (described below) should have it's own pool to help with cache locality when iterating through particles.

### Particle Camera

The engine will need to maintain a `ParticleCamera` object.  Every frame the game should update the origin, height, width, horizontal zoom factor, and vertical zoom factor to be consistent with the game's camera.  This allows particles to be rendered consistently in respect to the rest of your game's renderings.  

Multiple `ParticleCamera` instances can be maintained if the game itself has multiple cameras (e.g. split screen).

### MonoGameEmitterRenderGroup

Emitters in MonoGame are grouped together into `MonoGameEmitterRenderGroup` instances based on when they should all be drawn.  The render group maintains the spritebatch that is used for rendering to minimize render state changes when rendering multiple emitters.  These render groups should be created around the layering of the sprites of your game, so particles that should render on the foreground should be in a different group than particles that render in the background.

## Creating and Rendering Emitters

### Generating Emitter Logic Classes

In order for particle behavior to be added to your game, you must generate a C# implementation of the emitter logic definition (`.emlogic`) files that have been created, and these implementations must be added to your project.

<<To Be Added>>>

### Creating Emitters

Emitters can be created by instantiating a new `MonoGameEmitter` instance.  This instance takes in an instance of an emitter logic class (generated in the previous step), which `ParticlePool` it should pull particles from, what MonoGame `GraphicsDevice` it should use for its sprite batch, and the `ITextureFileLoader` implementation it should use.  

Once an emitter is created it should be added to the desired `MonoGameEmitterRenderGroup` instance via its `AddEmitter()` call.

Note that by default, emitters are not actively emitting new particles.  Your game must tell emitters to start emitting new particles by the `IsEmittingNewParticles` property.  Once set this property will remain active until you manual set it to `false`, except if a `One Shot` trigger is used (in which case the trigger will cause the emitter to deactivate itself).

If a `MonoGameEmitter` is created without a texture file defined, particles will default to a white square for a texture.

### Updating Emitters

Every frame of the game should have the `Update()` method called for every active emitter.  Not only will this cause active emitters to create new particles as required, but it also causes each particle that emitter is managing to have it's per-frame changes performed.  Even if an emitter is deactivated (i.e. `IsEmittingNewParticles == false`) its `Update()` method should still be called every frame so existing particles update as desired.

### Rendering Particles

Particles are rendered to the screen by calling `Render()` on the `MonoGameEmitterRenderGroup` instances.  This method takes an instance of a `ParticleCamera` to use for the view dimensions to use for rendering each emitter's particles.  

If multiple camera's are being used (and they each need to render particles) than each `MonoGameEmitterRenderGroup` should have its `Render()` called for each camera in use.

### Destroying Emitters

When an emitter is no longer needed it needs to be removed from any `MonoGameEmitterRenderGroup` instances it has been added to.  

Emitters also implement `IDisposable` and thus **must** have `Dispose()` called before removing references to that emitter.  If `Dispose()` is not called than its reservation in the `ParticlePool` is never released, and this will cause memory leaks.

It is important to keep in mind that disposing an emitter will immediately kill any existing particles that were tied to that emitter, and thus all of its particles will immediately disappear from the game.  In most cases, you will want to first set `IsEmittingNewParticles` to `false` so it does not create new particles, and then every frame call `emitter.CalculateLiveParticleCount()`.  Once this method returns `0` it is safe to dispose the emitter without disrupting the game screen.