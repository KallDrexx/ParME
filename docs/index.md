---
title: Welcome To Parme
sidebar: general
permalink: index.html
---

Parme (Particle Manipulation Engine) is a suite of tools and libraries that allow for rapid creation of particle effects for 2d games.

Currently Parme consists of:
* JSON based file format for defining behavior of emitters and particles
* Desktop WYSIWYG editor that supports rapid prototyping of particle effects, and exporting those behaviors
* Tools for converting Parme emission logic definition files into efficient C# code
* MonoGame rendering support
* Advanced integration for the [FlatRedBall Engine](http://flatredball.com)

<iframe width="560" height="315" src="https://www.youtube.com/embed/PQO8sO-6BWU" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>

## Design Philosophy

The core idea behind Parme is to make it easy for artists and game designers to create advanced particle effects in an engine and language agnostic manner.  This is done by decoupling the definition of how particles should be created and behave from the actual game technology it will be used in.  This allows for rapid prototyping of a large variety of particle effects while maintaining high performance in actual games.

The technological flexibility and high performance is achieved by relying on custom code generation.  This allows each language and engine to have their own final runtime for each particle effect, while ensuring each particle effect only has the logic it actually uses baked in, with minimal branching, minimal virtual calls, and maximum cache friendlyness.  If a specific behavior isn't being used by a particle, than that particle's runtime code will have zero code for that behavior in it.
 
## Concepts

Particle systems by combining emission behaviors and per-frame particle behaviors into a set of settings called an Emitter Logic Definition.  This is usually presented as a JSON file with a `.emlogic` file extension.  

A fully functioning emitter logic definition contains the following elements
* Particle lifetime - How long each particle that is created will live for
* Texture - Which filename of the texture the particles will use
* Texture Sections - Each emitter may have one or more sections of the texture to be rendered as.  This allows a particle to only use a portion of a larger sprite sheet, and defining multiple texture sections can allow for particle animations or each particle having a randomized look.
* Trigger - Defines the logic of when new particles will be created.  For example, will particles constantly emit every tenth of a second, based on how far the emitter has travelled, or just fire once and never again?
* Initializers - Defines properties of particles that are created every time a trigger occurs.  For example what velocity should they start at, where should they spawn (in relation to the emitter), how many should spawn, etc...
* Modifiers - Defines the behaviors of each particle that occurs every frame.  For example how much drag should each particle have, what type of acceleration/gravity should they be affected by, etc...

Triggers, initializers, and modifiers have a wide variety of behaviors to that can be mixed and matched with.   For more details, see their respective pages in the sidebar.