![hingeButton](https://user-images.githubusercontent.com/2615606/113931362-a9b4a480-97f2-11eb-96c4-ba42c087843d.gif)

# Hinge Button Prototype
Quick prototype for hinged buttons comissioned by Road To VR. 

Our goal with this test was to make a hand tracking button designed around a 'flicking' affordance rather than a 'pushing' one, since the lack of haptics with hand tracking often results in users 'pulling back' too soon when trying to press pushable buttons.

This prototype was put together in a day and has plenty of room for improvement:
* the interaction system doesn't destinguish the "closest" button to interact with. The current workaround is to shrink the button colliders to avoid the possibility of a finger pressing two buttons at once. 
* the "flip" reaction of the hinge buttons is a canned animation, but making them physics based could allow for interesting additional functionality like long presses, which could be usefull for things like holding down the backspace key in the numpad demo to delete all numbers in one go. 
