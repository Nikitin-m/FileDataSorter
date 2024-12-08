<h2>Laptop Configuration</h2>
<ul>
  <li>.NET 9</li>
  <li>Windows 11</li>
  <li>16 GB RAM</li>
  <li>Western Digital PC SN530 NVMe SSD</li>
  <li>AMD Ryzen 5 5500U @ 2.10 GHz</li>
</ul>

<h2>Sorting</h2>

<h3>FileSorterBoosted (split files by max file lines)</h3>
<table>
  <thead>
    <tr>
      <th>File Size</th>
      <th>Time</th>
      <th>Max File Lines</th>
      <th>Max Total Used Memory</th>
      <th>Uniq words / numbers</th>
    </tr>
  </thead>
  <tbody>
    <tr bold>
      <td><strong>1 GB</strong></td>
      <td><strong>32 s</strong></td>
      <td><strong>1,5 mln (≈100 mb)</strong></td>
      <td><strong>700 MB</strong></td>
      <td><strong>1_000_000</strong></td>
    </tr>
    <tr>
      <td>1 GB</td>
      <td>36 s</td>
      <td>1 mln</td>
      <td>500 MB</td>
      <td>1_000_000</td>
    </tr>
    <tr>
      <td>1 GB</td>
      <td>34 s</td>
      <td>5 mln</td>
      <td>1500 MB</td>
      <td>1_000_000</td>
    </tr>
    <tr>
      <td>20 GB</td>
      <td>1084 s (≈18 min)</td>
      <td>15 mln (≈1 gb)</td>
      <td>3400 MB</td>
      <td>1_000_000</td>
    </tr>
    <tr>
      <td>1 GB</td>
      <td>34 s </td>
      <td>1,5 mln (≈100 mb)</td>
      <td>2900 MB</td>
      <td>10_000_000</td>
    </tr>
    <tr>
      <td>1 GB</td>
      <td>38 s </td>
      <td>15 mln (≈1 gb)</td>
      <td>4100 MB</td>
      <td>10_000_000</td>
    </tr>
    <tr>
      <td>50 GB</td>
      <td>4800 s (80≈ min)</td>
      <td>15 mln (≈1 gb)</td>
      <td>4300 MB</td>
      <td>10_000_000</td>
    </tr>
  </tbody>
</table>

<h3>FileSorter(split files by size - less efficient)</h3>
<table>
  <thead>
    <tr>
      <th>File Size</th>
      <th>Time</th>
      <th>Batch Size</th>
      <th>Max Total Used Memory</th>
      <th>Uniq words / numbers</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>1 GB</td>
      <td>40 s</td>
      <td>100 MB</td>
      <td>4200 MB</td>
      <td>1_000_000</td>
    </tr>
    <tr>
      <td>20 GB</td>
      <td>1116 s (≈18 min)</td>
      <td>1 GB</td>
      <td>4200 MB</td>
      <td>1_000_000</td>
    </tr>
  </tbody>
</table>

<h3>InMemory(more efficient only when a lot of equal strings)</h3>
<table>
  <thead>
    <tr>
      <th>File Size</th>
      <th>Time</th>
      <th>Max Total Used Memory</th>
      <th>Uniq words / numbers</th>
    </tr>
  </thead>
  <tbody>
      <tr>
      <td>1 GB</td>
      <td>29 s</td>
      <td>600 MB</td>
      <td>1_000_000</td>
    </tr>
    <tr>
      <td>20 GB</td>
      <td>329 s (≈6m min)</td>
      <td>2600 MB</td>
      <td>1_000_000</td>
    </tr>
  </tbody>
</table>
