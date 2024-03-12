# Snake Structure

Contents:

* [Summary](#summary)
* [Discussion](#discussion)
* [Solution](#solution)
* [Result](#result)


## Summary

### Status
Accepted.

### Issue
We need to place obstacle 2x2, 2x1, 1x2, 1x1 in the array 2D
we don't need to loop random until the obstacle can be place to slot.

## Discussion

because if we random place the obstacle. the unlimit loop random will be occured.
and it lead to critical on performance if we need to spawn a ton of obstacle.


## Solution

  - find all empty slots.
  - shuffle slots. 
    - because we need to random 1 times.
  - try to place obstacle for each size. 
  - remove the block that's block the way

## Result

- acceptable. we can place obstacle in various size with out get any issue of performace, but need more improve the obstacle that's block the way on every case.