' 将一个工作簿分成多个工作簿
Sub splitWorkbook()
    Dim Arr, Rng As Range, Sht As Worksheet, Dic As Object
    Dim k, t, Str As String, i As Long, lastcolumn As Long
    Application.ScreenUpdating = False '关闭屏幕更新
    Arr = Range("A1").CurrentRegion.Value
    lastcolumn = UBound(Arr, 2) '求取最后一列的列号
    Set Rng = Rows(1) '标题行
    Set Dic = CreateObject("Scripting.Dictionary") '创建字典
    For i = 2 To UBound(Arr)
        Str = Arr(i, 1) '订单号，关键字
        If Not Dic.Exists(Str) Then '如果字典没有关键字
            Set Dic(Str) = Cells(i, 1).Resize(, lastcolumn) '把当前行装入到字典中
        Else '否则(字典中存在关键字)
            Set Dic(Str) = Union(Dic(Str), Cells(i, 1).Resize(, lastcolumn)) '把行连合起来
        End If
    Next
    k = Dic.Keys '字典关键字集合
    t = Dic.Items '字典项目集合
    On Error Resume Next
    With Sheets
        For i = 0 To Dic.Count - 1 '循环关键字的个数
            Set Sht = .Item(k(i)) '给变量赋值(工作表名为关键字)
            If Sht Is Nothing Then '该工作表不存在则插入一个空工作表
                .Add(After:=.Item(.Count)).Name = k(i) '新建的工作表将置于所有工作表之后，并命名为关键字
                Set Sht = ActiveSheet '活动工作表给变量
            Else '否则
                Sht.Cells.Clear '清除工作中所有内容和格式
            End If
            Rng.Copy Sht.Range("A1") '把标题写入第一行
            t(i).Copy Sht.Range("A2") '写入其他内容
            Sht.Cells.EntireColumn.AutoFit '自动调整全工作表单元格的列宽
            Set Sht = Nothing '变量处于初始状态
        Next
    End With
    Sheets(1).Activate '第1个工作表处于激活状态
    Application.ScreenUpdating = True '打开屏幕更新
End Sub