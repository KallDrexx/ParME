---
title: Triggers
permalink: general_triggers.html
---

Parme triggers define the circumstances that cause new particles to be created.  Parme comes with three types of triggers built in

* `One Shot` - Immediately creates new particles and causes the emitter to become inactive.  No more particles will be created until the emitter manually becomes active again, at which point it fires new particles once and goes inactive again.
* `Time Elapsed` -  Creates particles on a regular time interval while the emitter is active.
  * The `Frequencey` parameter defines how often (in seconds) particles should be created. 
* `Distance Based` - Creates particles every time the emitter has travelled a specified number of units
  * The `UnitsPerEmission` defines the magnitude of units the emitter must travel before particles will be created.
  
  