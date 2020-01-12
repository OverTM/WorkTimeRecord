/*VB
 * 
Sub 获取局域网指定IP电脑系统时间()
Dim temp
temp = CreateObject("WScript.Shell").Exec("net time \\192.168.1.101").StdOut.ReadLine
jzsj = CDate(Mid(temp, InStr(1, temp, "是") + 3))
Date = jzsj
Time = jzsj
End Sub

net use \\192.168.43.203 "Abc123456" /user:"Administrator" & net time \\192.168.43.203
解释：net use \\192.168.43.203 "Abc123456" /user:"Administrator"为与远程计算机建立连接，\\192.168.43.203为远程计算机的IP地址，"Abc123456"是远程计算机登录用户名"Administrator"的密码，&是命令之间的连接符。
 *
 */