#!/bin/sh
for filename in */*.js
do
    k6 run "${filename}"
done