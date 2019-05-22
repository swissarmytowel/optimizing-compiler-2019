import os

from functional import seq

head_file = './head.md'
tail_file = './tail.md'
opt_docs_directory = './OptimizationDocs'
base_bundle_file_name = 'bundle.md'
bundle_file_name = os.path.join(opt_docs_directory, base_bundle_file_name)

list_docs = []
for _, _, filenames in os.walk(opt_docs_directory):
    list_docs.extend(filenames)
    break

list_docs = seq(list_docs)\
            .filter(lambda x: 'bundle' not in x)\
            .sorted(key=lambda x: int(x.split('-')[0]))\
            .select(lambda x: os.path.join(opt_docs_directory, x))\
            .to_list()
list_docs.insert(0, head_file)
list_docs.append(tail_file)

with open(bundle_file_name, 'w') as bundle:
    for fname in list_docs:
        with open(fname) as infile:
            bundle.write(infile.read())
