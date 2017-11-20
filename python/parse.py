# pylint: disable=invalid-name

"Contains helper routines to aid in parsing benchmark output"
from collections import OrderedDict as od
from itertools import zip_longest

def distinct(iterable):
    "Makes the input list hold only distinct values"
    return list(od((x, 1) for x in iterable).keys())

def grouper(iterable, n, fillvalue=None):
    "Collect data into fixed-length chunks or blocks"
    args = [iter(iterable)] * n
    return zip_longest(*args, fillvalue=fillvalue)

def get_lines(it, s):
    "Gets lines from iterable starting with s"
    return [x[len(s):].replace(s, "") for x in it if x.startswith(s)]

def table_pair(group):
    "Parses markdown table from the benchmark output"
    keys = [x.strip() for x in group[0].split()]
    values = [float(x.replace(",", ".")) for x in group[2].split()]
    return dict(zip(keys, values))

def perform(output):
    "Parses benchmark output to a tree-like structure"
    implementations = get_lines(output, "# ")
    benches = get_lines(output, "## ")
    count = int(len(benches) / len(implementations))

    results = []
    for table in grouper(get_lines(output, "|"), 3):
        results.append(table_pair(table))

    data = dict(zip(implementations, grouper(zip(benches, results), count)))
    return data
