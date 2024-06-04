using tmpro;
using unityengine;
using unityengine.events;
using unityengine.inputsystem;

public class uiinventory : monobehaviour
{
    public itemslot[] slots;

    public gameobject inventorywindow;
    public transform slotpanel;
    public transform dropposition;

    [header("selected item")]
    private itemslot selecteditem;
    private int selecteditemindex;
    public textmeshprougui selecteditemname;
    public textmeshprougui selecteditemdescription;
    public textmeshprougui selecteditemstatname;
    public textmeshprougui selecteditemstatvalue;
    public gameobject usebutton;
    public gameobject equipbutton;
    public gameobject unequipbutton;
    public gameobject dropbutton;

    private int curequipindex;

    private playercontroller controller;
    private playercondition condition;

    void start()
    {
        controller = charactermanager.instance.player.controller;
        condition = charactermanager.instance.player.condition;
        dropposition = charactermanager.instance.player.dropposition;

        controller.inventory += toggle;
        charactermanager.instance.player.additem += additem;

        inventorywindow.setactive(false);
        slots = new itemslot[slotpanel.childcount];

        for (int i = 0; i < slots.length; i++)
        {
            slots[i] = slotpanel.getchild(i).getcomponent<itemslot>();
            slots[i].index = i;
            slots[i].inventory = this;
            slots[i].clear();
        }

        clearselecteditemwindow();
    }

    public void toggle()
    {
        if (inventorywindow.activeinhierarchy)
        {
            inventorywindow.setactive(false);
        }
        else
        {
            inventorywindow.setactive(true);
        }
    }

    public bool isopen()
    {
        return inventorywindow.activeinhierarchy;
    }

    public void additem()
    {
        itemdata data = charactermanager.instance.player.itemdata;

        if (data.canstack)
        {
            itemslot slot = getitemstack(data);
            if (slot != null)
            {
                slot.quantity++;
                updateui();
                charactermanager.instance.player.itemdata = null;
                return;
            }
        }

        itemslot emptyslot = getemptyslot();

        if (emptyslot != null)
        {
            emptyslot.item = data;
            emptyslot.quantity = 1;
            updateui();
            charactermanager.instance.player.itemdata = null;
            return;
        }

        throwitem(data);
        charactermanager.instance.player.itemdata = null;
    }

    public void throwitem(itemdata data)
    {
        instantiate(data.dropprefab, dropposition.position, quaternion.euler(vector3.one * random.value * 360));
    }

    public void updateui()
    {
        for (int i = 0; i < slots.length; i++)
        {
            if (slots[i].item != null)
            {
                slots[i].set();
            }
            else
            {
                slots[i].clear();
            }
        }
    }

    itemslot getitemstack(itemdata data)
    {
        for (int i = 0; i < slots.length; i++)
        {
            if (slots[i].item == data && slots[i].quantity < data.maxstackamount)
            {
                return slots[i];
            }
        }
        return null;
    }

    itemslot getemptyslot()
    {
        for (int i = 0; i < slots.length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    public void selectitem(int index)
    {
        if (slots[index].item == null) return;

        selecteditem = slots[index];
        selecteditemindex = index;

        selecteditemname.text = selecteditem.item.displayname;
        selecteditemdescription.text = selecteditem.item.description;

        selecteditemstatname.text = string.empty;
        selecteditemstatvalue.text = string.empty;

        for (int i = 0; i < selecteditem.item.consumables.length; i++)
        {
            selecteditemstatname.text += selecteditem.item.consumables[i].type.tostring() + "\n";
            selecteditemstatvalue.text += selecteditem.item.consumables[i].value.tostring() + "\n";
        }

        usebutton.setactive(selecteditem.item.type == itemtype.consumable);
        equipbutton.setactive(selecteditem.item.type == itemtype.equipable && !slots[index].equipped);
        unequipbutton.setactive(selecteditem.item.type == itemtype.equipable && slots[index].equipped);
        dropbutton.setactive(true);
    }

    void clearselecteditemwindow()
    {
        selecteditem = null;

        selecteditemname.text = string.empty;
        selecteditemdescription.text = string.empty;
        selecteditemstatname.text = string.empty;
        selecteditemstatvalue.text = string.empty;

        usebutton.setactive(false);
        equipbutton.setactive(false);
        unequipbutton.setactive(false);
        dropbutton.setactive(false);
    }

    public void onusebutton()
    {
        if (selecteditem.item.type == itemtype.consumable)
        {
            for (int i = 0; i < selecteditem.item.consumables.length; i++)
            {
                switch (selecteditem.item.consumables[i].type)
                {
                    case consumabletype.health:
                        condition.heal(selecteditem.item.consumables[i].value); break;
                    case consumabletype.hunger:
                        condition.eat(selecteditem.item.consumables[i].value); break;
                }
            }
            removeselcteditem();
        }
    }

    public void ondropbutton()
    {
        throwitem(selecteditem.item);
        removeselcteditem();
    }

    void removeselcteditem()
    {
        selecteditem.quantity--;

        if (selecteditem.quantity <= 0)
        {
            if (slots[selecteditemindex].equipped)
            {
                unequip(selecteditemindex);
            }

            selecteditem.item = null;
            clearselecteditemwindow();
        }

        updateui();
    }

    public bool hasitem(itemdata item, int quantity)
    {
        return false;
    }
}