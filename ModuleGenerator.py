import json
import codecs
import os


with codecs.open('cfg.json', 'r', 'utf-8') as read_file:
	data = json.load(read_file)
	Guid = str(data['Guid']) # @Guid_
	Name = str(data['Name']) # @Name_
	Title = str(data['Title']) # @Title_

# Создание директории модуля
try: os.mkdir('./{0}'.format(Name))
except: pass

# Загрузка шаблона вьюмодели
ViewModel = codecs.open('_temp/Temp_ViewModel.cs', 'r', 'utf-8').read().replace('@Guid_', Guid).replace('@Name_', Name).replace('@Title_', Title)
# Сохранение шаблона вьюмодели
codecs.open('{0}/{0}ViewModel.cs'.format(Name), 'w', 'utf-8').write(ViewModel)

# Загрузка шаблона XAML
ViewXaml = codecs.open('_temp/Temp_Window.xaml', 'r', 'utf-8').read().replace('@Guid_', Guid).replace('@Name_', Name).replace('@Title_', Title)
# Сохранение шаблона XAML
codecs.open('{0}/{0}Window.xaml'.format(Name), 'w', 'utf-8').write(ViewXaml)

# Загрузка шаблона XAML.CS
View = codecs.open('_temp/Temp_Window.xaml.cs', 'r', 'utf-8').read().replace('@Guid_', Guid).replace('@Name_', Name).replace('@Title_', Title)
# Сохранение шаблона XAML.CS
codecs.open('{0}/{0}Window.xaml.cs'.format(Name), 'w', 'utf-8').write(View)