---
title: Modifiers
permalink: general_modifiers.html
---

Modifiers are behaviors that are performed on particles every frame.  

Parme has the following modifiers built in:

* Animating Texture - This causes the particle to progress from the first texture section to the last throughout its lifetime, allowing for the particles to be animated.  This will cause a linear time progression between all the sections, with the particle starting with the first texture section and hitting the last texture shortly before the end of its lifetime.
* Constant Acceleration - This modifies the particle's velocity by the specified `X` and `Y` amounts every second
* Constant Size - This modifies the particle's height and width based on the specified amount every second.
* Drag - Adds drag to the particle
* Ending Color Multiplier - This is the color multiplier that the particle should be at at the end of its lifetime.  This will cause a linear interpolation between the starting color multiplier to the end based on the particle's lifetime.

 