#  ParME (Particle Manipulation Engine)

**This project is still under heavy early development**

Parme is a suite of engine agnostic libraries and tools to make it easy to design and manage emission and particles for 2d games.  

Currently ParME contains:

* Json based file format for defining behavior or emitters and particles
* Desktop WYSIWYG editor for designing emitters and particle behaviors
* Tools for converting a Parme emission logic file into efficient C# code
* Monogame rendering support
* Advanced integration for the [FlatRedBall Engine]()

[![Demonstration Video](https://i.imgur.com/0D3TIQA.png)](https://www.youtube.com/watch?v=PQO8sO-6BWU "Youtube Video")

## Design Philosophy

The core idea behind Parme is to make it easy for artists and game designers to create advanced particle effects in an engine and language agnostic manner.  This is done by decoupling the definition of how particles should be created and how they should behave from the actual game technology it will be used in.

The flexibility in defining how particles behave is done while minimizing branching and virtual calls by relying on code generation.  If a specific behavior isn't desired for a set of particles than that behavior won't exist in the particle's code at all.  


