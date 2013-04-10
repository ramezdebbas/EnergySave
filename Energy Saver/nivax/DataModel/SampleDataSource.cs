using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The data model defined by this file serves as a representative example of a strongly-typed
// model that supports notification when members are added, removed, or modified.  The property
// names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs.

namespace PlanningDairyTemplate.Data
{
    /// <summary>
    /// Base class for <see cref="SampleDataItem"/> and <see cref="SampleDataGroup"/> that
    /// defines properties common to both.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class SampleDataCommon : PlanningDairyTemplate.Common.BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public SampleDataCommon(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            this._uniqueId = uniqueId;
            this._title = title;
            this._subtitle = subtitle;
            this._description = description;
            this._imagePath = imagePath;
        }

        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return this._uniqueId; }
            set { this.SetProperty(ref this._uniqueId, value); }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        private string _subtitle = string.Empty;
        public string Subtitle
        {
            get { return this._subtitle; }
            set { this.SetProperty(ref this._subtitle, value); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value); }
        }

        private ImageSource _image = null;
        private String _imagePath = null;
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(SampleDataCommon._baseUri, this._imagePath));
                }
                return this._image;
            }

            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        public void SetImage(String path)
        {
            this._image = null;
            this._imagePath = path;
            this.OnPropertyChanged("Image");
        }
    }

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class SampleDataItem : SampleDataCommon
    {
        public SampleDataItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content, SampleDataGroup group)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this._content = content;
            this._group = group;
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }

        private SampleDataGroup _group;
        public SampleDataGroup Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }
        private string _createdon = string.Empty;
        public string CreatedOn
        {
            get { return this._createdon; }
            set { this.SetProperty(ref this._createdon, value); }
        }
        private string _createdtxt = string.Empty;
        public string CreatedTxt
        {
            get { return this._createdtxt; }
            set { this.SetProperty(ref this._createdtxt, value); }
        }

        private string _Colour = string.Empty;
        public string Colour
        {
            get { return this._Colour; }
            set { this.SetProperty(ref this._Colour, value); }
        }
        private string _bgColour = string.Empty;
        public string bgColour
        {
            get { return this._bgColour; }
            set { this.SetProperty(ref this._bgColour, value); }
        }
        private string _createdontwo = string.Empty;
        public string CreatedOnTwo
        {
            get { return this._createdontwo; }
            set { this.SetProperty(ref this._createdontwo, value); }
        }
        private string _createdtxttwo = string.Empty;
        public string CreatedTxtTwo
        {
            get { return this._createdtxttwo; }
            set { this.SetProperty(ref this._createdtxttwo, value); }
        }

        private string _currentStatus = string.Empty;
        public string CurrentStatus
        {
            get { return this._currentStatus; }
            set { this.SetProperty(ref this._currentStatus, value); }
        }
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class SampleDataGroup : SampleDataCommon
    {
        public SampleDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
        }

        private ObservableCollection<SampleDataItem> _items = new ObservableCollection<SampleDataItem>();
        public ObservableCollection<SampleDataItem> Items
        {
            get { return this._items; }
        }
        
        public IEnumerable<SampleDataItem> TopItems
        {
            // Provides a subset of the full items collection to bind to from a GroupedItemsPage
            // for two reasons: GridView will not virtualize large items collections, and it
            // improves the user experience when browsing through groups with large numbers of
            // items.
            //
            // A maximum of 12 items are displayed because it results in filled grid columns
            // whether there are 1, 2, 3, 4, or 6 rows displayed
            get { return this._items.Take(12); }
        }
    }

    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// </summary>
    public sealed class SampleDataSource
    {
        private static SampleDataSource _sampleDataSource = new SampleDataSource();

        private ObservableCollection<SampleDataGroup> _allGroups = new ObservableCollection<SampleDataGroup>();
        public ObservableCollection<SampleDataGroup> AllGroups
        {
            get { return this._allGroups; }
        }

        public static IEnumerable<SampleDataGroup> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");
            
            return _sampleDataSource.AllGroups;
        }

        public static SampleDataGroup GetGroup(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static SampleDataItem GetItem(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public SampleDataSource()
        {
           // String ITEM_CONTENT = String.Format("");

            var group1 = new SampleDataGroup("Group-1",
                    "Commands & Actions",
                    "Commands & Actions",
                    "Assets/Images/10.jpg",
                    "Reduce Your Power Consumption With These Steps Before you get started thinking about going off the grid and using renewable sources of energy to power your home, it's important to estimate how much energy you use so you can figure out the size generator you will need.");
            group1.Items.Add(new SampleDataItem("Group-1-Item-1",
                    "Renewable",
                    "Why You Should Choose Renewable Energy Using renewable energy is something that many people are starting to become more and more interested in. The reasons are clear -- we absolutely cannot rely on fossil fuels for the world's energy much longer!",
                    "Assets/DarkGray.png",
					"",            
                    "Details:\n\nWhy You Should Choose Renewable Energy Using renewable energy is something that many people are starting to become more and more interested in. The reasons are clear -- we absolutely cannot rely on fossil fuels for the world's energy much longer! Not only are they becoming more expensive, they're also damaging to the earth in more ways than one. Many people cannot afford the oil prices these days. It doesn't matter whether you are in one of the poorest countries in the world, or in one of the most affluent -- people are having trouble paying for fossil fuels. That's not even to mention the fact that the methods of getting these fossil fuels can be quite damaging to the earth, and to the workers who get the fuel. Once the fuel is used, it can sometimes be damaging to the environment.\n\nFossil fuels are becoming so harmful to the world, in fact, that there are wars being fought over them. People are needlessly giving up their lives in order to provide these fossil fuels for the world's energy. Unfortunately, there is no clear way to stop this problem except for finding different sources of energy.\n\nThere are two renewable sources of energy that are your best bets these days. It's actually not as difficult to implement as many people believe it to be -- these sources of energy renew time and time again. We are talking about the sun, and the wind! The sun is probably the most commonly thought of source, and solar panels are even popular in some products that can be purchased today. The sun comes out reliably each morning, and it can be an excellent way to gather energy for your entire home, or for travel.\n\nIf you live in a windy area, you can certainly make use of the wind in order to produce energy. This energy can also be used to power the electricity in your home. While many people are intrigued about these renewable sources of energy, it's surprising that more people have not hopped on board. Luckily, with the introduction of certain products that make it easy to get started creating solar panels and wind power generators, the movement to create cheaper, better, energy for power is rapidly catching on. \n\nMake it your mission to become a part of this movement so you can live off the grid. The cost of getting started with sun or wind energy is very small compared to what many people are paying for their monthly energy bills! It's time to take action, and make use of renewable sources of energy instead.",
                    group1) { CreatedOn = "Group", CreatedTxt = "Commands & Actions", CreatedOnTwo = "Item", CreatedTxtTwo = "Renewable", bgColour = "#6495ED", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/11.jpg")), CurrentStatus = "Energy Saver" });
            group1.Items.Add(new SampleDataItem("Group-1-Item-2",
                     "Power Consumption",
                     "Reduce Your Power Consumption With These Steps Before you get started thinking about going off the grid and using renewable sources of energy to power your home, it's important to estimate how much energy you use so you can figure out the size generator you will need.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nReduce Your Power Consumption With These Steps Before you get started thinking about going off the grid and using renewable sources of energy to power your home, it's important to estimate how much energy you use so you can figure out the size generator you will need. The good news is that most of this information can be found on your monthly power bill. You'll be able to determine how much energy you have used in the last month, as well as your average usage per month. Many utility companies include graphs so you can see how your usage has differed across different months of the year. This accounts for changes in the season, and changes in your usage patterns.\n\nIt is most important to pay attention to your highest month. That's because whatever renewable energy source you choose, you need to be sure it can handle your highest need for energy. The next thing you need to do is learn more about building solar panels or wind power generators. Then you can determine the size of the system you'll need to build or purchase, and see what your costs will be. Purchasing these systems can be quite expensive (though they pay for themselves in the long run over continuing to pay monthly for electricity bills), but they are worth it in the long run.\n\nIf you consider yourself to be handy, you might want to build solar panels or a wind power generator yourself. This might sound scary, but if you have clear directions it will likely be a lot easier for you to get started with. Luckily, there are some guides out on the market now that can be incredibly useful for you. Once you have the right guide, you can start to determine the size you'll need and how much you might want to spend. Portable power generators that you build can cost as little as $200. It easily, and quickly, pays for itself. Obviously, the more power you need, the bigger solar panel or wind generator you'll need. \n\nFiguring out your power consumption is one of the first steps for a reason. Different households use different amounts of power, so it's best to determine this in advance to see if it will be feasible for you to invest in renewable sources of energy for your home. If you're like most families, you'll find that this will quickly pay for itself over time, and you'll feel much better about the power you use. This will save you money, and it is a much friendlier method for the earth!",
                     group1) { CreatedOn = "Group", CreatedTxt = "Commands & Actions", CreatedOnTwo = "Item", CreatedTxtTwo = "Power Consumption", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/12.jpg")), CurrentStatus = "Energy Saver" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-3",
                     "Solar Power",
                     "Why Choose Solar Power? In this series, we have briefly discussed the need for renewable energy sources. Many people will choose solar energy as their source -- with good reason. However, before you choose for your household, you need to understand what sets solar energy apart from others.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nWhy Choose Solar Power? In this series, we have briefly discussed the need for renewable energy sources. Many people will choose solar energy as their source -- with good reason. However, before you choose for your household, you need to understand what sets solar energy apart from others. Obviously, using solar energy is excellent for the earth. Using fossil fuels damages the earth as the fuels are gathered, as well as when they're used. Clearly, this is not sustainable over the long term. It is people like you who will help make the earth a better place to live by using these renewable sources instead!\n\nAnother motivating reason is because you are spending a lot on energy bills per month. If you start using solar energy, you'll be getting energy for free instead! All you need to worry about are the startup costs, and the time it takes to build your own solar energy power system. There are great guides out on the market that can help you with this option -- and it is a lot easier than you might be thinking. Still, there are some times where it is not a good idea for you to think about solar energy. One such circumstance is if you study other renewable sources and find that another fits your needs better than solar. Wind energy is a good example of this. Most people will find that solar energy suits them just fine if they live in areas that are not windy enough.\n\nIf you're a worried that solar energy could not possibly power your entire home, you're not alone. Many people have this fear because it is so unknown in so many parts of the world. Rest assured that as long as you have enough large solar panels, you can certainly power your entire home! It's even easier, and cheaper, when you just want to use solar energy for travel purposes, such as with an RV, a boat, or a variety of other reasons. You owe it to yourself to look into using solar power as much as possible. It is a lot easier to get started than you might be thinking, and it is certainly a lot cheaper and better for the environment. Many of the supplies can even be found at your local hardware store. Others can be found online -- making it easier than ever to get started with this renewable energy source.\n\nAll you need now are some step-by-step directions and you'll be well on your way to having free, reliable power without having to rely on fossil fuels and your local power company (or their high bills!).",
                     group1) { CreatedOn = "Group", CreatedTxt = "Commands & Actions", CreatedOnTwo = "Item", CreatedTxtTwo = "Solar Power", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/13.jpg")), CurrentStatus = "Energy Saver" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-4",
                     "How Solar Power Works?",
                     "If you're thinking of using solar power for your home's electricity, you might be curious as to how it works in the first place. Some of these facts will be obvious, and other parts not so obvious. Basically, the sun contains a lot of energy that it brings to the Earth's surface every single day.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nIf you're thinking of using solar power for your home's electricity, you might be curious as to how it works in the first place. Some of these facts will be obvious, and other parts not so obvious. Basically, the sun contains a lot of energy that it brings to the Earth's surface every single day. This energy is freely available, and fairly reliable. That means it just makes good sense to put this to use as your source of power as soon as possible! You might be wondering why we don't just all use solar power to begin with, if it is so cheap and easy to use. That's because constructing the solar panels can be a difficult upfront cost to swallow. If this had been made the norm earlier, however, the chances are good the prices of solar panels would be cheaper, and they would be more readily available. Thankfully, if you have the right sources, you can construct the solar panels yourself and literally save thousands of dollars over time on your power bills.\n\nOnce you do have solar panels, they will collect the energy the sun puts out. Your solar power source will then convert the energy to give you a electricity. The panels that are used are called photovoltaic cells. These cells are conductors to ease the spread of the electricity. The combination of the sun's rays on the conductors causes a chemical reaction to take place. The energy is absorbed and the electrons are able to break free of their atoms to create electricity.\n\nDon't worry if this all sounds too complicated! It's a good idea to have a basic understanding of how it works -- but all you really need to know is that it does work. This is a far better method of powering your home than using fossil fuels and depending on the local power company! The next step is making sure you have a good source to purchase solar panels, as well as step-by-step directions for the rest of the materials you will need. Don't think that this will be too expensive for you to get started with -- you can get started with less than $200 for a portable panel. Larger panels will obviously be more expensive, but the cost savings over long term makes it more than worth it.\n\nThere's no doubt about it -- the sun contains more than enough energy to go around. It's time to harness the power of the sun and to make use of it for cheaper, better energy.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Commands & Actions", CreatedOnTwo = "Item", CreatedTxtTwo = "How Solar Power Works?", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/14.jpg")), CurrentStatus = "Energy Saver" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-5",
                     "Pros and Cons",
                     "You want to make the absolute best decision for you and your household. That means weighing both the positive and negative aspects of converting your home to solar power. The first disadvantage is that the cost of purchasing solar power generation, or building solar panels yourself, can be prohibitive for many people.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nYou want to make the absolute best decision for you and your household. That means weighing both the positive and negative aspects of converting your home to solar power. The first disadvantage is that the cost of purchasing solar power generation, or building solar panels yourself, can be prohibitive for many people. It is often more palatable to just continue paying your monthly utility bill than it is to lay down a large upfront cost for renewable energy. The good news is that it is probably a lot less expensive than you think, and the long term cost savings makes it worth it! There are also methods you can use to get solar panels for free! It's it's hard to believe, but there are two sources where you are likely to get the solar panels at no cost. Even if you want to build them yourself, you can save money over the cost of the materials and still come out with a high-quality solar power generator.\n\nAnother issue might be if you live in an area where the sun is not always out in full force. If you need energy for a large home, this is not likely to work for you if you are in an area where the sun doesn't always shine. This can be particularly difficult in the wintertime, or in the rainy season. It can also be an issue if your setup does not powerful enough, or converts energy inefficiently. Some people also believe that solar power is less efficient than other methods out there. Some energy is lost through the conversion stage, and other power sources do a better job of maintaining this energy so that it is used most efficiently.\n\nDon't let those downsides dissuade you! There are an incredible number of benefits that come along with using solar power. For one thing, it is possible to get set up for free, or for low cost, as was already mentioned. You'll also find that this type of power generation is very quiet and safe.\n\nIt is also good news that there are step-by-step guides available that can help you through the process of converting your home to solar power. Putting these guides to use is very important so you know exactly what to do, and you save time and energy by doing it right the first time.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Commands & Actions", CreatedOnTwo = "Item", CreatedTxtTwo = "Pros and Cons", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/15.jpg")), CurrentStatus = "Energy Saver" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-6",
                     "How to Build?",
                     "How to Build Your Own Solar Generator Now that you've made the decision to convert your home to solar energy, you're probably a little nervous to try and build your own solar generator.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nHow to Build Your Own Solar Generator Now that you've made the decision to convert your home to solar energy, you're probably a little nervous to try and build your own solar generator. You'll have to follow some detailed instructions to get the best results, but here is a brief overview of the things you'll need to do. First you have to realize that there are different kinds of systems. Some are portable, and others are more permanent and are bigger to provide more energy. The portable models can be made for under $200. This is a great way to travel, or even to power smaller things in your home. For illustration purposes, we will be focusing on a portable model in this example. For more detailed examples, and to build a solar generator on a larger scale, you'll need a fully detailed guide.\n\nThe first thing you'll need to do is gather supplies. The main component of this will be your solar panels. For portable versions,12v is enough. You'll also need a charge controller, battery, and inverter. As for your solar panel, you can purchase this, get it used, or make your own. There are different costs associated with each, so be sure to weigh that when you make your decision as to which one to get. Keep in mind that the quality of solar panel can determine how efficient it is at converting the sun's energy into power for your use. Then, you'll have to put the pieces together to make your travel solar generator. Clearly, this is not something you can do without detailed, step-by-step directions. Better yet -- it's best to learn by video where someone can demonstrate what you need to do it in real time.\n\nJust a few short years ago, it would have been impossible to try and do this as there were not very many good tutorials on how to do so. These days there are a few great guides on the market that will help you get started right away. Whether you want to build the portable version described in this piece, or you want to build a solar generator on a larger scale, the right guide can help you do so.\n\nIt's important to read reviews to see which one will be right for you. Not only do you need to choose the right solar generator, you also need to choose the right size so you have all the power you need.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Commands & Actions", CreatedOnTwo = "Item", CreatedTxtTwo = "How to Build?", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/16.jpg")), CurrentStatus = "Energy Saver" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-7",
                     "Being Economical",
                     "Being Economical With Renewable Power These days many of us are trying to find ways to be more economical. Times are tough financially all over the world, so this is more important than ever. If you examine your monthly bills, you'll probably find that a large chunk is going to your utility costs.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nBeing Economical With Renewable Power These days many of us are trying to find ways to be more economical. Times are tough financially all over the world, so this is more important than ever. If you examine your monthly bills, you'll probably find that a large chunk is going to your utility costs. While this is an essential charge, it certainly doesn't have to be that way! It's a much better idea to use renewable energy sources, or live off the grid. This can seem scary or radical, but it is a great way to save money. It is also a good way to help save the environment, as fossil fuels are damaging both when they are extracted, and when there are in use. Caring about the planet, and your wallet, is definitely a good motivator to make use of renewable power like solar and wind power. In fact, solar and wind power are the two most common options. Many people who live in windier areas are able to create wind turbines to generate power. These are also commonly referred to as windmills in many cases. These can be large, or small depending on how much power they need to emit. The downside to these is that you need a large area to build them that is free from obstruction of the wind.\n\nSolar power is also incredibly common, and is a good alternative for those who do not live in an area that is windy enough. These make use of photovoltaic panels in order to convert the solar energy into energy you can use to power items. You have to keep in mind that solar energy can be limited, and not as powerful. Still, it is fairly reliable and very popular for a reason.\n\nNow, you can certainly purchase a wind power generator or solar power panels through companies who make them. You will end up spending thousands of dollars, which can be difficult to swallow. It is becoming a much better option to make these items yourself! This can seem overwhelming, but it is actually easier than you think. In fact, most of the materials are available at your local hardware store. Others are available online -- and you can even get certain components for free, or for a massive discount. It's time to ditch your utility bill and become more economical by using renewable power. You'll be helping to save the earth, and saving money over the long run by using one of these choices.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Commands & Actions", CreatedOnTwo = "Item", CreatedTxtTwo = "Being Economical", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/17.jpg")), CurrentStatus = "Energy Saver" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-8",
                     "Build a Wind Power",
                     "How to Build a Wind Power Generator If you've read a little bit about wind power generators, the chances are good that you are ready to get one yourself. Unfortunately, those that are sold on the market can often come with hefty price tags in the thousands of dollars.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nHow to Build a Wind Power Generator If you've read a little bit about wind power generators, the chances are good that you are ready to get one yourself. Unfortunately, those that are sold on the market can often come with hefty price tags in the thousands of dollars. Many people who are trying to save money do not have this kind of up front cash, so it's important to learn how to do this without breaking the bank. This method will not be for everyone, but a great way to save money is to build your own wind power generator. If you're building a smaller one, you can do this for under $200! This will certainly not power your entire home, but it'll do a great job of powering something in your household. Every little bit helps! You can do this on a larger scale, though it will obviously cost more money.\n\nBefore you dive too deeply into the world of wind power generators, you do need to check your area for how much wind is available. Look for the average wind speed, and be sure that it will be enough to power what you need to power. If you find that your areas is not windy enough, you will want to try solar power instead. Either way, you'll be using an excellent renewable energy source that will save you money over the long term. \n\nIf you have decided that you are able to use a wind power generator, it's time to get started gathering the components you will need! This is not a complete list, but some basics are a battery, tower, body, and other items. Clearly, you will need a step-by-step guide to help you put these components together. They can also be helpful to watch full instructional videos to make sure you do not miss a single step. While not all of these items will be familiar to you, they are actually quite easy to acquire. You can find most of them in your local hardware store. There are some parts that will be cheaper to order on line. Many people have had success using discount sites like eBay.com where you can get the parts you need for this. This is always worth looking into, as you probably want to build your wind power generator on as much of a budget as possible.\n\nAfter you have built your wind power generator you will breathe a sigh of relief because you are free from the power of the utility companies forever. It's time to take action on this today, so you can start to save money and be kinder to our planet tomorrow.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Commands & Actions", CreatedOnTwo = "Item", CreatedTxtTwo = "Build a Wind Power", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/18.jpg")), CurrentStatus = "Energy Saver" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-9",
                     "Wind Power",
                     "What is a Wind Power Generator? Many people focus on the sun as the main source of renewable energy. However, wind power can be quite efficient and a great choice. This will not work in all areas, but for those who are able to choose, the benefits are quite substantial.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nWhat is a Wind Power Generator? Many people focus on the sun as the main source of renewable energy. However, wind power can be quite efficient and a great choice. This will not work in all areas, but for those who are able to choose, the benefits are quite substantial.Before discussing much more about wind power, you need to determine if it is an option for you. It is essential to make sure that the area you live in is windy enough to make the wind generator work. You also need to make sure that you have an area that is large enough to set up something that will be able to power your home. You can also set up smaller wind generators that will power lesser areas.\n\nThe good news is that you probably need less wind than you think! Many areas generate enough wind to make your wind generator work. Figure out what your average wind speed is in your to help you make your decision. Another part of the decision will be how much money you'll need to spend to get started. If you're creating a smaller version, you can probably get started with less than $200. If you're working on a larger scale, you will need to spend more money. Just like with solar power, using wind power will more than make up for the cost of the materials upfront over the long term. This was briefly mentioned, but it is worth another mention. You need to make sure that you have enough space for your wind turbine. It's important to have a large yard (an acre or more) and a wide open space. This, unfortunately, rules out many homes where there are smaller yards or or obstructions that will block the airflow. It should also be said that if you are trying to build a wind power generator to power your entire home you will need to check with your local area to see if you are allowed to do so. Some locales prohibit building large fixtures like this without a permit.\n\nIf you make it through all of that, wind power generators can be a great thing! They can be very reliable and efficient. Best of all -- once it is installed, there are no additional costs. Wind is freely available, and it's an awesome feeling to be free from paying utility bills for the rest of your life. This is one investment you really can't pass up if you want to make use of renewable energy like the wind.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Commands & Actions", CreatedOnTwo = "Item", CreatedTxtTwo = "Wind Power", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/19.jpg")), CurrentStatus = "Energy Saver" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-10",
                     "Wind Power Work",
                     "How Does Wind Power Work? If you're considering wind power as a source of renewable energy, you need to understand how it works. It's so easy to just flick on a light switch without thinking about where the energy comes from.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nHow Does Wind Power Work? If you're considering wind power as a source of renewable energy, you need to understand how it works. It's so easy to just flick on a light switch without thinking about where the energy comes from. Unfortunately, in most households, it comes from fossil fuels, which are not renewable sources of energy. Sources like wind power are renewable, and are freely available for all. All you need is a wind power generator to take advantage of this natural and free resource. Before you get started with that, however, you should understand how wind power works. Basically, the wind creates kinetic energy. This is done through the use of rotor blades, which move the winds energy into kinetic energy. These rotor blades rotate around the shaft and move the energy down into the generator. The generator then generates electricity because its magnets are rotated around a conductor.\n\nDon't worry if this explanation sounds overly technical to you. There is no need to be worried about how wind power works -- all you need to know is that it does work, and it works well. Of course, you do have to be in an area that is windy enough. You can check with your local weather service for average wind speeds for each season. Many areas have more than enough wind, so this may not be a concern for you. Now that you have an understanding of how wind power works, you might be wondering why everyone doesn't just start using wind power. One of the reasons is because of low wind situations, as was described above. Another reason is because it is not very commonly done in personal households. Thankfully, this trend is changing with the new guides out on the market that can help you get started doing this!\n\nYet another reason this is not commonly done is because people don't know how to get started. It can seem incredibly daunting to try and find a wind power generator, or to build one on your own. That's why you need to have a step-by-step manual, and videos, so this becomes a non-issue.\n\nSoon enough, you'll be the envy of everyone in your area! No one wants to have to pay hefty utility bills each month. When you have your own wind power generator, this is not something you'll have to deal with ever again. After you have paid for the wind generator, or built it yourself, you will be able to have free power for life.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Commands & Actions", CreatedOnTwo = "Item", CreatedTxtTwo = "Wind Power Work", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/20.jpg")), CurrentStatus = "Energy Saver" });
					 
            this.AllGroups.Add(group1);


			
			
         
        }
    }
}
