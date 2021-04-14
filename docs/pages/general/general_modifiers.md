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

In addition to normal modifiers, there is a special purpose modifier called a **Position Modifier**.  These modifiers override the default position logic of a particle for custom functionality.  The current position modifiers that are built in are:

* Altitude bounce modifier - This causes the particle to bounce in the positive Y axis (even when rotated).  The particle will bounce when it hits the same Y coordinate that the particle would be at if there was no bounce effect in play.

 