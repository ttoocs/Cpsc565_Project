#define __NO_STD_VECTOR // Use cl::vector instead of STL version

#include <stdio.h>
#include <time.h>

#include <fstream>
#include <iostream>
#include <string>
#include <iterator>

#define WITH_MAIN

#ifdef WITH_MAIN
int main(int argc, const char * argv[]){
  
}
#endif

//TODO: BRAIN STRUCTURE:
// Define a neruon - has linked lists to other neruons (with weight, possibly negative), and a trigger value
// Define an input neruon - has triggers based on external world
// Define an output neron - has some-sort of output.

// Have "good"/"bad" training.
// Methoid1: Simply whatever is currently happening is good, reinforce the last X-time neruons vice-versa for bad.
// Methoid2: Nothing.
// Methoid3: Simulated enviroment to quickly train the brain.

// Note: How dose it ever get new neruons? - Just constantly growing, but also pruning?
// Note: How about synapses?

// Mutations:
// 1: More neruons (More energy loss)
// 2: More edges (More energy loss)
// 3: More inputs - less inputs.

// Handling differnt/modifications to body:
// Assumption: Differnt inputs/outputs, simply appended.
// 1: Simply put it the most similar to before? (Trys to fix/correct for different I/O)
// 2: Screw it, it's a new creature. 
// 3: Re-train it with methoid3 above.

