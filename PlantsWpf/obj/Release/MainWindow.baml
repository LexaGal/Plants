   M S B A M L   `   `   `  ����   	PlantsWpf    PlantsWpf.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35I EWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=nullF 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  Sensors system��$	��365��$	��525�� � Icon$ TreeIcon.ico��.��� -    �Name$ RootGrid��$��-1,0,1,0q�
������� � ColumnDefinitions �� $
��425* ��� $	��92* �.+�]� $
��Left=�$��46����.  $��1,5,0,0q�$	��Top=�$	��157��.+��� -   $��Start��$��Start��{�.  $y�	Collapsed=�$
��Left=�$��10,9,0,0q�$	��Top=�$��54��$��26���� -   $��Pause��$��Pause��$y�	Collapsed=�$
��Left=�$��27��$��10,9,0,0q�$	��Top=�$��56���� -   $��
WeatherBox��{�.  $��10,9,0,0q�$
��Left=�$��61��$��27��$	��Top=��� -   $��AddArea��$��Add area��{�.  $
��Left=�$��10,9,0,0q�$	��Top=�$��66��$��28���� $
��Auto=�$
��Auto=�$��
0,51,0,-15q�$��2��$��0��.��� -   $ 
PlantsGrid�� �� $
��257* ��� $
��260* ��� $��0�� K� ZIndex$ 99��$��0,0,114,283q�.+�9� -   $��LogIn�� 9� 
IsExpanded . $��Log in / Register��$��26�� �� Left$	 152��
 �� Top$ 15��$	��156��.���� $ 99��
������$	��252��$��
0,0,-198,0q� J� Effect 3  -System.Windows.Media.Effects.DropShadowEffect  .+��� -   	 ��Name$	 
EmailLabel��$��30��$ 22��$ 11��$��Email���� $��30��$ 52��$��Password��
 J� RenderTransformOrigin$
 
0.45,1.867&�$ 9���� -	    ��Name$ Email��$!��galushkin.aleksey@gmail.com��
�������$��29��$	 114��$ 22��$	��234��G� -
    G�Name$ Password�� G� Password$ qYYfO8Di12345!��
�������$��29��$	 114��$ 53��$	��234��A� -    A�Name$ RegisterGrid��$��95��$ 8��$ 85��$	��340��$y�	Collapsed=�.+��� $��
First name��$��3,30,45,-28q��� $��	Last name��$
 	0.8,0.433&�$��-110,64,159,-62q��� -   $	 ConfirmPasswordLabel��$��Confirm Password��$��-225,0,232,2q��� -   $ 	FirstName��$��Alexey��
�������$��106,0,-227,3q��� -   $ LastName��$��	Galushkin��
�������$��-7,34,-114,-31q�G� -   $ ConfirmPassword��$ qYYfO8Di12345!��
�������$��-121,-32,0,35q��� -    ��Name$ LoginButton��$��Submit��$ 8��$	 222��$��75��$��23���� -    ��Name$ CreateAccount��$��Create account��$	 252��$ 4���� -   $	 Logginglabel��$y�Hidden=�$
��Left=�$
��Left=�$.��(You are being logged in. Please, wait...��
���  ��$ 9��$	 189��$	��337��$��32���� -   $��LoginNameLabel��$��Center=�$�� ��$��316,12,10,0q�$	��Top=�$��2��$��0��$��Stretch=�$
��Auto��