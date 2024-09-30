<div align="center">
  <h1 align="center">Petafly</h1>
  <h3>Transfer files up to a Petabyte! (If you have a file with that size, of course ;)</h3>
  
<div align="justify">
This is a File Transfer program, where instead of uploading your files to a cloud server, and then other users download your files from that server. You are the server!
That means that you and the other user must have both devices online to download!
The good part is that there's no file limit, you can upload anything you want! And your internet speed won't be limited by another service!
<p></p>
  
<div align="center">
<h2>
Installation Tutorial:
</h2>
<div align="justify">
Coming Soon...
But <p></p>
<a href="https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-8.0.8-windows-x64-installer?cid=getdotnetcore">Install .NET from here:</a><p></p>
<a href="https://www.python.org/ftp/python/3.12.6/python-3.12.6-amd64.exe">Install Python from here:</a><p></p>
<a href="https://bin.equinox.io/c/bNyj1mQVY4c/ngrok-v3-stable-windows-amd64.zip">Install Ngrok from here:</a>

<div align="center">
<h2>
Release Notes:
</h2>
  
<div align="left">
<p>
<b>- PublicExperimentalTest 1:</b> 
  <dd><li type="disc">Core program created, allowing to share files through the same network, to any device connected to it, but extremely buggy.</li></dd>
  <dd><li type="disc">Only Spanish Support.</li></dd>
  <dd><li type="disc">First basic interface added.</li></dd>
  <p><p>
    
<b>- PublicExperimentalTest 2:</b>
      <dd><li type="disc">Added colors to the interface.</li></dd>
      <dd><li type="disc">Cleaner design of the console to make everything easier for the final user.</li></dd>
  <p><p>
    
<b>- PublicExperimentalTest 3:</b> 
      <dd><li type="disc">Fixed all color interface bugs.</li></dd>
      <dd><li type="disc">Added a clear screen command after an important action to prevent text and banners from accumulating on top of others.</li></dd>
      <dd><li type="disc"> Fixed a bug when opening more than one port, because default port is 80, and the console opened 80 all the time causing a glitch. Now when you open 2 or more HTTP Servers, the first port will be 80, then 8000, and after this just +1 every time (8000, 8001, 8002, 8003, 8004...) </li></dd>
      <dd><li type="disc">Fixed several exceptions that made the program crash in certain situations without giving an error, confusing the user.<p></li></dd>
     

<b>v.0.0.4:</b> 
      <dd><li type="disc">Moving out from Public Experimental Tests phase! Petafly is updating pretty fast and somehow it's useful in specific situations, it's not 100% useless.</li></dd>
      <dd><li type="disc">Removing installer MSI from releases, until further development.</li></dd>
      <dd><li type="disc">Deleted dependencies checker, it's not reliable because of program updating and for people who updates this every week is just not worth it, I will just put the dependencies download links here in the Installation tutorial of the README</li></dd>
      <dd><li type="disc">Fixed a bug where you could start a HTTP Server with port 80 (default http port) in the first option after creating a Sharable Folder, and then when opening a port for an existing folder in option 2, the server also started with port 80, instead of port 8000 or 81, breaking everything</li></dd>
