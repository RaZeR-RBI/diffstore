# pylint: disable=invalid-name

"""
Plots the benchmark data. If a path to output file is defined, reads it,
otherwise runs the benchmark by itself.
"""
import json
import sys
import subprocess
import matplotlib.pyplot as plt
import numpy as np
import pandas as pd
import seaborn as sns

def run():
    "Runs the 'dotnet run' command and captures output"
    command = 'dotnet run --no-build -p ../Diffstore.Benchmark'
    return subprocess.check_output(command, shell=True)

def read(path):
    "Reads the benchmark output from file"
    with open(path, 'r') as f:
        contents = f.read()
    return contents

def filter_samples(iterable):
    samples = np.array(list(filter(lambda x: x < 100.0, iterable)))
    median = np.median(samples)
    stddev = np.std(samples)
    lower = median - 3 * stddev
    upper = median + 3 * stddev
    return [x for x in samples if lower < x < upper]

def frame(labels, values):
    "Creates a pandas DataFrame from labels and arrays of different length"
    d = dict(zip(labels, values))
    return pd.DataFrame(dict([(k, pd.Series(v)) for k, v in d.items()]))

output = read(sys.argv[1]) if len(sys.argv) == 2 else run()
graphs = json.loads(output)["Value"]

sns.set_style("whitegrid")
for title, categories in graphs.items():
    labels = categories.keys()
    values = list(map(filter_samples, list(categories.values())))
    data = frame(labels, values)
    f, ax = plt.subplots(figsize=(8, 8))
    sns.swarmplot(data=data)
    f.savefig('images/' + title + '.png')
