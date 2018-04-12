# Telenor Data Server
这是一个为挪威`Mytos`公司写的一个sftp服务，它运行在Linux上，由Cron调度。它会同步Telenor的sftp的文件。
读取并写入MongoDB中，虽然它是`.Net Core`程序，但是它目前只能在Linux上运行，因为部分代码直接调用了Linux命令。

This is an sftp service for Norway `Mytos` Company. It runs on Linux and is scheduled by Cron. It will synchronize Telenor to sftp file. 
Read and write to MongoDB, although it is `.Net Core` program, but it can currently only run on Linux, because part of the code directly calls the shell
