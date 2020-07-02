import json
import codecs
import os
import pyodbc
import re 

def CreateModuleInDB(name):

	with codecs.open('connection.json', 'r', 'utf-8') as read_file:
		data = json.load(read_file)

		connectionstr = 'DRIVER={0};SERVER={1};DATABASE={2};UID={3};PWD={4}'.format(
			"{SQL Server\}",
			str(data['SERVER']), # SERVER, 
			str(data['DATABASE']), # DATABASE, 
			str(data['User']), # User, 
			str(data['Password'])) # Password
		
		print(connectionstr)

	cnxn = pyodbc.connect(connectionstr) # Подключение
	cursor = cnxn.cursor()

	sqlGetGuid = "SELECT TOP (1) [Guid] FROM [GZ].[dbo].[SystemModules] WHERE [Title] LIKE N'{0}' AND IsDeleted = 0;".format(name)
	sqlAddModule = "INSERT INTO [dbo].[SystemModules]([Guid],[Type],[Title],[DateCreated],[IsDeleted],[DateDeleted]) VALUES (NEWID(), N'4A3BEF77-574D-472F-9502-060E20F01B43', N'{0}', SYSDATETIMEOFFSET(), 0, NULL);".format(name)

	cursor.execute(sqlGetGuid)
	result = cursor.fetchone()

	if result == None: 
		cursor.execute(sqlAddModule)
		cnxn.commit()  

		cursor.execute(sqlGetGuid)
		result = cursor.fetchone()

	guid = result[0]


	return guid


with codecs.open('cfg.json', 'r', 'utf-8') as read_file:
	data = json.load(read_file)
	# Guid = str(data['Guid']) # @Guid_
	Name = str(data['Name']) # @Name_
	Title = str(data['Title']) # @Title_

Guid = CreateModuleInDB(Title)

# Создание директории модуля
try: os.mkdir('./{0}'.format(Name))
except: pass

# Загрузка шаблона вьюмодели
ViewModel = codecs.open('_temp/Temp_ViewModel.cs', 'r', 'utf-8').read().replace('@Guid_', Guid).replace('@Name_', Name).replace('@Title_', Title)
# Сохранение шаблона вьюмодели
codecs.open('Modules/{0}/{0}ViewModel.cs'.format(Name), 'w', 'utf-8').write(ViewModel)

# Загрузка шаблона XAML
ViewXaml = codecs.open('_temp/Temp_Window.xaml', 'r', 'utf-8').read().replace('@Guid_', Guid).replace('@Name_', Name).replace('@Title_', Title)
# Сохранение шаблона XAML
codecs.open('Modules/{0}/{0}Window.xaml'.format(Name), 'w', 'utf-8').write(ViewXaml)

# Загрузка шаблона XAML.CS
View = codecs.open('_temp/Temp_Window.xaml.cs', 'r', 'utf-8').read().replace('@Guid_', Guid).replace('@Name_', Name).replace('@Title_', Title)
# Сохранение шаблона XAML.CS
codecs.open('Modules/{0}/{0}Window.xaml.cs'.format(Name), 'w', 'utf-8').write(View)