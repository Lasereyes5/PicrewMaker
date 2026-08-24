# PicrewMaker

⚠️ NOT OFFICIAL: This is a third-party local renderer, not affiliated with the original service. Please check the [DISCLAIMER.md](DISCLAIMER.md).

------------

An non-official local picrew maker for maker downloaded and packed by [Picrew Downloader Bookmarklet](https://viatrix.computer/picrew-downloader/) (for unzipped folder).

The program only parses specific maker folders which unzipped from [Picrew Downloader Bookmarklet](https://viatrix.computer/picrew-downloader/) downloaded maker file (.ora file)

### Usage

1. Add the bookmarklet `Download entire picrew maker bookmarklet` in website [Picrew Downloader Bookmarklet](https://viatrix.computer/picrew-downloader/).
2. Goto the picrew website, find the site of maker you want to download, click the bookmarklet.
3. Wait for downloading maker.
	- If download process be interrupted by ad, network problem or other reasons, just refresh the site and try again.

4. Find the downloaded maker file (`[maker id].ora`), unzip it to a folder. (no root folder inside .ora file)
5. Open program, drag the entire folder (or `stack.xml` inside the folder) into the program, wait program loading.

The program imitate the interface and most of functions of picrew maker website, so it should not be hard to use.

### Tips
- The bookmarklet downloaded file won't keep any information about author or maker name (it will keep description though).
	- **Strongly adviced** to rename the unzipped maker folder into format `[name] - [author]` or `[maker id]_[name] - [author]` or etc. for better recognizability, also for respecting authors.
- You can copy and rename (or even share) the `stack.xml` inside the same maker folder to "save" different maker state, you can also drag in the different xml state file into program to load the specific state.
	- Click the `Done` button, change the maker or close the program will save the state into xml state file, so please ensure you did these operations before doing anything about the xml state file. (`stack.xml` by default)