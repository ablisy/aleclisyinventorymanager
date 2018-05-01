# aleclisyinventorymanager
Unity Editor for creating and managing item assets. 
Allows level designers to create items while automatically making the items adhere to coding practices of the project.

Open with Item Management > Item Manager

Create a category and add variables that every item in the category will use.

Click the category and add an item to the category. The item will inherit from the category script, and will implement the categories variables.
You can choose that particular items values for each of the public variables as well.

EXAMPLE:
Create Rifles category.
Make a string for Manufacturer, a float for Weight, an Int for Damage, and a bool for new.
Create an Item in the Rifles category that has a Manufacturer of Springfield, weight of 12.34, damage of 12, and a true bool for new.

Note: The way to add more variables is not ideal right now. You will have to add lines in several spots marked with:
//!!!ADD ADDITIONAL VARIABLES HERE!!!
to add more variable types such as Vector2.
I would like to make the add process easier in the future, but for now it works and needed areas are marked.

Things left to do:
Add the ability to create a prefab for each item.
Clean up the backend so that it is easier to add new categories.
Make the Window prettier (this often takes a little while so I haven't done it here)
