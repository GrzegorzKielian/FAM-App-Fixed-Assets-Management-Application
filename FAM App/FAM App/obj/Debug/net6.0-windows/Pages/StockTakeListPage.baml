   M S B A M L   `   `   `  ����B  >FAM App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:FAM_App.PagesFAM_App.Pages  5      %    FAM_App.Pages.StockTakeListPage   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -localclr-namespace:FAM_App.Pages     5        N� Title$  StockTakeListPage��5	       � 	Resources 5         dark �� 
TargetType&   System.Windows.Controls.DataGrid �� Property �� Value  darkCombobox  	darkLabel��%^  &	        &	 N     &	 �    �� 5   
   "  6   .�5      ��  � �� �#1522386+   5      �� 5   
   " ��6%   .�5      ��  � ��$ 1��6,   �� 5       � ��$
 Left=�64   �� 5       � ��$ Center=�62   �� 5       � ��$ Tahoma)�6+   �� 5       � ��$ 15(�6)   �� 5       � ��$ Stretch=�64   �� 5       � ��$ 25��6'   �� 5       � ��$ 0,5,0,10q�6'   5      �� 5   
   " ��6"   .�5      ��  � �� �#dddddd6+   �� 5       � ��$ Tahoma)�6+   �� 5       � ��$ 15(�6)   5      5      .��5      �  � ColumnDefinitions 5    
   �� 5!      $
��0.1* �6   �� 5"      $
��0.9* �6    � RowDefinitions 5$   
   �� 5%      $��* �6   .+�5'   
   �� -   5         ��Name$ BorderPanel��5'      
�������$��1q�6?   ���#152238$��0��5)      $��1��6!   $��1q�5*       �� Padding$ 1q�6   ._�5,      �  5-      �� 5.      $
��0.2* �6$   �� 5/      $
��0.6* �6$   �� 50      $
��0.2* �6$   .+�52      �� $��1��6   .+�54      �� #��[ 6   .��55      �� $'��!Data rozpoczęcia inwentaryzacji:��6$   $
��Wrap=�6L   56      �� 57      -   5        	 ��Name$	 StocktakeDateStart_Box��57      #��[ 6?   58      �� 5:      -   5        
 ��Name$
 ShowStocktakeList_Button��5:      $��Pokaż wykaz��6?   $��35��6U   $��15(�6a   $��Tahoma)�6o   6�   �� 5;      -   5        $
 ExportToPDF_Button��5;      $��Zapisz do PDF��69   $��30��6Q   $��15(�6]   $��Tahoma)�6k   $��0,20,0,0q�6   6�   5<      5=      5>      �� 5?   
   -   5        $ 
BorderPage��5?      $��1��6&   $��1��66   $��2��6C   $ 2q�6W   ._�5@      �  5A      �� 5B      $��0.15* �6$   �� 5C      $��0.85* �6$   .+�5E        -   5          Name$ StocktakeDataGrid��5E   (   $��0��6   #��[  6C   $��5q�6a     5F      -   5        $ StocktakeListDataGrid��5F   (   $��1��6   #��[  6G   $��5q�6e   5G      5H      5I      5J      