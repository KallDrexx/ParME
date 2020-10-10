---
title: Initializers
permalink: general_initializers.html
---

Initializers in Parme define the properties that particles will have when they are created.  In order for particles to be visible an emitter logic definition requires a particle count initializer, a color multiplier initializer, and a size initializer to be defined.

## Categories

There are quite a few different categories that initializers have based on what properties of a particle it manages.  It is expected that each emitter logic definition will only have at most one of each category.

### Particle Count Initializers

These initializers define how many particles are created every time a trigger is fired.  This is required for particles to show up, as without one zero particles will be created every time the trigger fires.  Parme comes with the following particle count initializers built in:

* Static - A statically defined number of particles are created each time.  
* Random - A random number of particles between a defined minimum and maximum are created each time.

### Color Multiplier Initializer

These define the starting RGBA multiplier values a particle starts with.  These color values are between `0` and `255` for the RGB components, while the Alpha component is a value between `0.0` and `1.0`.  A color multiplier must be defined for particles to be initially visible, as without one the RGBA values will be all zeros (transparent and no color).  Parme comes with the following color multiplier initializers built in:

* Static - A statically defined initial set of RGBA values for particles to have when they are created

### Size Initializer

These define the starting size of each particle being created.  This is required for particles to show up, as without any size initializers the particles end up with a size of 0x0.  Parme comes with the following size initializers built in:

* Static - All particles will have a single width and height value 
* Random - Particle will have a starting size between a minimum and maximum width and height.  It also has an option to preserve the aspect ratio to prevent deformations of the texture if that's required.

### Position Initializer

These define where new particles should be created in relation to its emitter.  If no positional initializer is specified than all new particles will be created at the exact position of the emitter.  Parme comes with the following initializers built in:

* Static - Particles will be created at the single defined X/Y offset from the emitter's current position
* Region - Particles will be created within a rectangular region offset from the emitter.

###  Velocity Initializer

These define the initial velocity in 2d space that particles will have when they are created.  If no initializer is specified then all new particles will have a velocity of `(0, 0)` and thus not move.  Parme comes with the following initializers built in:

* Random Range - Particles will have a velocity between the specified minimum and maximum `X` and `Y` values
* Radial / Wedge - Particles will have a velocity pushing them away from their originating point between the specified minimum and maximum degrees with a specific magnitude of speed.

### Rotational Velocity Initializer

These define the initial rotational velocity that particles will have (i.e. how fast and in which direction they spin).  If no initializer is specified then all new particles will have no rotational speed, and thus not rotate.  Parme comes with the following initializers built in:

* Static - Particles will have an initial rotational velocity equal to the specified number of degrees per second
* Random - Particles will have an initial rotational velocity between the specified minimum and maximum degrees per second

### Texture Section Index Initializer

These define which texture index a new particle should use.  By changing which texture section a particle uses it can allow variety in the particles that get emitted.  If no initializer is specified than `Single`  is the default. Parme comes with the following initializers built in:

* Single - All particles will always use the first texture section that's been defined
* Random _ Particles will be given a random texture section out of all the sections that have been defined.

