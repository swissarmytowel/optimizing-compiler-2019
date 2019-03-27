#!/bin/sh

mono --runtime=v4.0 Gplex.exe /unicode SimpleLex.lex
mono --runtime=v4.0 Gppg.exe /no-lines /gplex SimpleYacc.y