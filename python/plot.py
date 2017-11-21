# pylint: disable=invalid-name,redefined-outer-name
"""
Plots the benchmark data. If a path to output file is defined, reads it,
otherwise runs the benchmark by itself.
"""
import json
import os
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
    "Filters the input samples by removing samples beyond 10 and 90 percentiles"
    samples = np.array(iterable)
    lower = np.percentile(samples, 10)
    upper = np.percentile(samples, 90)
    return [x for x in samples if lower < x < upper]

def frame(labels, values):
    "Creates a pandas DataFrame from labels and arrays of different length"
    d = dict(zip(labels, values))
    return pd.DataFrame(dict([(k, pd.Series(v)) for k, v in d.items()]))

def extract(categories):
    labels = categories.keys()
    values = list(map(filter_samples, list(categories.values())))
    return frame(labels, values)

output = read(sys.argv[1]) if len(sys.argv) == 2 else run()
graphs = json.loads(output)["Value"]

sns.set_style("whitegrid")
if not os.path.exists('images/'):
    os.makedirs('images/')
for title, categories in graphs.items():
    data = extract(categories)
    f, ax = plt.subplots(figsize=(8, 8))
    f.suptitle(title)
    ax.set(xlabel='Implementation', ylabel='Time in milliseconds')
    sns.swarmplot(data=data)
    f.savefig('images/' + title + '.png')
