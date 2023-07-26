using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using System.Data.Common;
using System.Net;
using System.Runtime.Remoting;
using System.Security.Policy;
using System.Xml.Linq;

namespace RCInjector
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            Private TargetProcessHandle As Integer
    Private pfnStartAddr As Integer
    Private pszLibFileRemote As String
    Private TargetBufferSize As Integer

    Public Const PROCESS_VM_READ = &H10
    Public Const TH32CS_SNAPPROCESS = &H2
    Public Const MEM_COMMIT = 4096
    Public Const PAGE_READWRITE = 4
    Public Const PROCESS_CREATE_THREAD = (&H2)
    Public Const PROCESS_VM_OPERATION = (&H8)
    Public Const PROCESS_VM_WRITE = (&H20)
    Dim DLLFileName As String
    Public Declare Function ReadProcessMemory Lib "kernel32"(_
    ByVal hProcess As Integer, _
    ByVal lpBaseAddress As Integer, _
    ByVal lpBuffer As String, _
    ByVal nSize As Integer, _
    ByRef lpNumberOfBytesWritten As Integer) As Integer

    Public Declare Function LoadLibrary Lib "kernel32" Alias "LoadLibraryA"(_
    ByVal lpLibFileName As String) As Integer

    Public Declare Function VirtualAllocEx Lib "kernel32"(_
    ByVal hProcess As Integer, _
    ByVal lpAddress As Integer, _
    ByVal dwSize As Integer, _
    ByVal flAllocationType As Integer, _
    ByVal flProtect As Integer) As Integer

    Public Declare Function WriteProcessMemory Lib "kernel32"(_
    ByVal hProcess As Integer, _
    ByVal lpBaseAddress As Integer, _
    ByVal lpBuffer As String, _
    ByVal nSize As Integer, _
    ByRef lpNumberOfBytesWritten As Integer) As Integer

    Public Declare Function GetProcAddress Lib "kernel32"(_
    ByVal hModule As Integer, ByVal lpProcName As String) As Integer

    Private Declare Function GetModuleHandle Lib "Kernel32" Alias "GetModuleHandleA"(_
    ByVal lpModuleName As String) As Integer

    Public Declare Function CreateRemoteThread Lib "kernel32"(_
    ByVal hProcess As Integer, _
    ByVal lpThreadAttributes As Integer, _
    ByVal dwStackSize As Integer, _
    ByVal lpStartAddress As Integer, _
    ByVal lpParameter As Integer, _
    ByVal dwCreationFlags As Integer, _
    ByRef lpThreadId As Integer) As Integer

    Public Declare Function OpenProcess Lib "kernel32"(_
    ByVal dwDesiredAccess As Integer, _
    ByVal bInheritHandle As Integer, _
    ByVal dwProcessId As Integer) As Integer

    Private Declare Function FindWindow Lib "user32" Alias "FindWindowA"(_
    ByVal lpClassName As String, _
    ByVal lpWindowName As String) As Integer

    Private Declare Function CloseHandle Lib "kernel32" Alias "CloseHandleA"(_
    ByVal hObject As Integer) As Integer
    Dim ExeName As String = IO.Path.GetFileNameWithoutExtension(Application.ExecutablePath)
    Private Sub Inject()
        On Error GoTo 1 ' If error occurs, app will close without any error messages 
        Timer1.Stop()
        Dim TargetProcess As Process() = Process.GetProcessesByName(TextBox1.Text)
        TargetProcessHandle = OpenProcess(PROCESS_CREATE_THREAD Or PROCESS_VM_OPERATION Or PROCESS_VM_WRITE, False, TargetProcess(0).Id)
        pszLibFileRemote = OpenFileDialog1.FileName
        pfnStartAddr = GetProcAddress(GetModuleHandle("Kernel32"), "LoadLibraryA")
        TargetBufferSize = 1 + Len(pszLibFileRemote)
        Dim Rtn As Integer
        Dim LoadLibParamAdr As Integer
        LoadLibParamAdr = VirtualAllocEx(TargetProcessHandle, 0, TargetBufferSize, MEM_COMMIT, PAGE_READWRITE)
        Rtn = WriteProcessMemory(TargetProcessHandle, LoadLibParamAdr, pszLibFileRemote, TargetBufferSize, 0)
        CreateRemoteThread(TargetProcessHandle, 0, 0, pfnStartAddr, LoadLibParamAdr, 0, 0)
        CloseHandle(TargetProcessHandle)
1:      Me.Show()
    End Sub
            InitializeComponent();
        }

        private void Browse_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            For i As Integer = (Dlls.SelectedItems.Count - 1) To 0 Step - 1
            Dlls.Items.Remove(Dlls.SelectedItems(i))
        Next
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            Timer1.Interval = 2
        Timer1.Start()
        RadioButton1.Checked = True
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Timer1.Start()
        CheckBox1.Checked = True
        CheckBox1.Enabled = False
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            If IO.File.Exists(OpenFileDialog1.FileName) Then

       End If
       Dim TargetProcess As Process() = Process.GetProcessesByName(TextBox1.Text)
        If TargetProcess.Length = 0 Then

            Me.Label1.Text = ("İnject İçin Bekleniyor " + TextBox1.Text + ".exe" + "....")
        Else
            Timer1.Stop()
            Me.Label1.ForeColor = Color.Green
            Me.Label1.Text = "İnject Tamamlandı!"
            Call Inject()
            If CheckBox1.Checked = True Then
                Me.Close()
            Else
            End If
        End If

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Dlls.Items.Clear()
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void DLLs_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Timer1.Stop()
        CheckBox1.Checked = True
        CheckBox1.Enabled = True
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog1.Filter = "DLL (*.dll) |*.dll"
        OpenFileDialog1.ShowDialog()
        Dim FileName As String
        FileName = OpenFileDialog1.FileName.Substring(OpenFileDialog1.FileName.LastIndexOf("\"))
        Dim DllFileName As String = FileName.Replace("\", "")
        Me.Dlls.Items.Add(DllFileName)
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            If IO.File.Exists(OpenFileDialog1.FileName) Then
         Dim TargetProcess As Process() = Process.GetProcessesByName(TextBox1.Text)
            If TargetProcess.Length = 0 Then
                Me.Label1.ForeColor = Color.Red
                Me.Label1.Text = ("İnject İçin Bekleniyor " + TextBox1.Text + ".exe" + "....")
            Else
                Timer1.Stop()
                Me.Label1.ForeColor = Color.Green
                Me.Label1.Text = "İnject Tamamlandı!"
                Call Inject()
                If CheckBox1.Checked = True Then
                    Me.Close()
                End If


            End If
        End If
        }
    }
}


