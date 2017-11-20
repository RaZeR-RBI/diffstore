# pylint: disable=invalid-name

"""
Plots the benchmark data. If a path to output file is defined, reads it,
otherwise runs the benchmark by itself.
"""
import sys
import subprocess
import pprint
import parse

def run():
    "Runs the 'dotnet run' command and captures output"
    command = 'dotnet run -p ../Diffstore.Benchmark'
    lines = subprocess.check_output(command, shell=True).splitlines()
    return [x.decode() for x in lines]

def read(path):
    "Reads the benchmark output from file"
    with open(path, 'r') as f:
        contents = [x.strip('\n') for x in f.readlines()]
    return contents

output = read(sys.argv[1]) if len(sys.argv) == 2 else run()
data = parse.perform(output)

pp = pprint.PrettyPrinter(indent=4)
pp.pprint(data)
