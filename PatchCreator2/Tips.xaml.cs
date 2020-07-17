using System;
using System.Windows;

namespace PatchCreator2
{
    /// <summary>
    /// Interaction logic for TipsWindow.xaml
    /// </summary>
    public partial class TipsWindow : Window
    {
        private static string[] quotes = {
            @"So if you want to go fast, if you want to get done quickly, if you want your code to be easy to write, make it easy to read.
- Robert C. Martin",
            @"A long descriptive name is better than a short enigmatic name. A long descriptive name is better than a long descriptive comment.
- Robert C. Martin",
            @"If you're good at the debugger it means you spent a lot of time debugging. I don't want you to be good at the debugger.
- Robert C. Martin",
            @"You should name a variable using the same care with which you name a first-born child.
- Robert C. Martin",
            @"Clean code is not written by following a set of rules. You don’t become a software craftsman by learning a list of heuristics. Professionalism and craftsmanship come from values that drive disciplines.
- Robert C. Martin",
            @"Every time you write a comment, you should grimace and feel the failure of your ability of expression.
- Robert C. Martin",
            @"Of course bad code can be cleaned up. But it’s very expensive.
- Robert C. Martin",
            @"Why do most developers fear to make continuous changes to their code? They are afraid they’ll break it! Why are they afraid they’ll break it? Because they don’t have tests.
- Robert C. Martin",
            @"It is not the language that makes programs appear simple. It is the programmer that make the language appear simple!
- Robert C. Martin",
            @"When you are working on a problem, you sometimes get so close to it that you can’t see all the options. You miss elegant solutions because the creative part of your mind is suppressed by the intensity of your focus. Sometimes the best way to solve a problem is to go home, eat dinner, watch TV, go to bed, and then wake up the next morning and take a shower.
- Robert C. Martin",
            @"There are two parts to learning craftsmanship: knowledge and work. You must gain the knowledge of principles, patterns, practices, and heuristics that a craftsman knows, and you must also grind that knowledge into your fingers, eyes, and gut by working hard and
practicing.
- Robert C. Martin",
            @"Clean code always looks like it was written by someone who cares.
- Robert C. Martin",
            @"Redundant comments are just places to collect lies and misinformation.
- Robert C. Martin",
            @"Programmers must avoid leaving false clues that obscure the meaning of code.
- Robert C. Martin",
            @"When you see commented-out code, delete it!
- Robert C. Martin",
            @"There are only two hard things in Computer Science: cache invalidation and naming things.
- Phil Karlton",
            @"We like to think we spend our time power typing, but we actually spend most of our time staring into the abyss.
- Douglas Crockford",
            @"Obsolete comments are worse than no comments.
- Douglas Crockford",
            @"You are responsible for the quality of your code.
- Michael Toppa, 10 Tips for clean code
You are responsible. Not your client. Not your boss. You don’t tell your doctor to skip washing their hands because it saves time. Standards and quality are important.",
            @"Use meaningful names.
- Michael Toppa, 10 Tips for clean code",
            @"Write code that expresses intent.
- Michael Toppa, 10 Tips for clean code",
            @"Comments are often lies waiting to happen. Code should speak for itself whenever possible.
- Michael Toppa, 10 Tips for clean code",
            @"Boy Scout Rule: Leave the code better than you found it.
- Michael Toppa, 10 Tips for clean code",
            @"Follow the single Responsible Principle.
- Michael Toppa, 10 Tips for clean code
 Each code component you write, be it a class or a method, should have one responsibility, just one thing that it does and does well.",
            @"Work in short cycles: incremental and iterative
- Michael Toppa, 10 Tips for clean code
Don’t work in monolithic, six-month increments. Instead, work on small, independent portions of functionality, and ship and test often. More importantly, be committed to a culture of ongoing, iterative customer feedback.",
            @"Practice, Practice, Practice! Musicians don’t only play in front of an audience.
- Michael Toppa, 10 Tips for clean code",
            @"Any fool can write code that a computer can understand. Good programmers write code that humans can understand.
- Martin Fowler",
            @"Experience is the name everyone gives to their mistakes.
- Oscar Wilde",
            @"Perfection is achieved not when there is nothing more to add, but rather when there is nothing more to take away.
- Antoine de Saint-Exupery",
            @"Code is like humor. When you have to explain it, it’s bad.
- Cory House",
            "A good programmer looks both ways before crossing a one-way street.",
            "Hey! It compiles! Ship it! (Not really though)",
            "Pasting code from the Internet into production code is like chewing gum found in the street.",
            "Whatever you are patching, I hope you wrote a test for it.",
            "Did you know? You can set the IDE to run code anlysis at the commit changes window before you commit.",
            "Did you run the linter on your changes? how about you go and do that now.",
            @"One may say reliability is improved by writing tests, therefore code coverage of high precent indicates high reliability.
Not necessarily, reliability is better increased by writing, not more, but better tests.
So, did you write a test today?",
            "Extensibility – The quality of being designed to allow the addition of new capabilities or functionality.",
            "Maintainability –The ease with which a product can be maintained in order to: correct defects or their cause.",
            "Readability – The ease with which a human reader can comprehend the purpose, control flow, and operation of source code.",
            "Weeks of coding can save you hours of planning.",
            "Be nice to your assigned operation team PMO.",
            "Before you continue and go about your day, did you communicate everything you needed to the feature's stakeholders?",
            "Go ahead, press CTRL + ALT + L.",
            @"I love deadlines. I like the whooshing sound they make as they fly by.
- Douglas Adams",
            @"Never trust a computer you can’t throw out a window.
- Steve Wozniak",
            @"Nobody expects the Spanish Incquisition!
- Monty Python's Flying Circus",
            "Developer: an organism that turns coffee into code.",
            @"Simplicity is prerequisite for reliability.
- Edsger W. Dijkstra",
            @"Program testing can be used to show the presence of bugs, but never to show their absence!
- Edsger W. Dijkstra",
            @"Your obligation is that of active participation. You should not act as knowledge-absorbing sponges, but as whetstones on which we can all sharpen our wits.
- Edsger W. Dijkstra",
            @"Leaving nots in a conditional like that twists my mind around at a painful angle.
- Martin Fowler",
            @"Repetition is the root of all software evil.
- Martin Fowler",
            @"If you have to spend effort looking at a fragment of code and figuring out what it's doing, then you should extract it into a function and name the function after the what.
- Martin Fowler",
            @"Code that communicates its purpose is very important. I often refactor just when I'm reading some code. That way as I gain understanding about the program, I embed that understanding into the code for later so I don't forget what I learned.
- Martin Fowler",
            @"Clean code is simple and direct. Clean code reads like well-written prose. Clean code never obscures the designers’ intent but rather is full of crisp abstractions and straightforward lines of control.
- Grady Booch",
            @"You want it in one line? Does it have to fit in 80 columns?
- Larry Wall",
            @"If I had more time, I would have written a shorter letter.
- Cicero",
            @"Fix the cause, not the symptom.
- Steve Maguire",
            @"Always code as if the guy who ends up maintaining your code will be a violent psychopath who knows where you live.
- John Woods",
            @"Use the force, Harry
- Gandalf",
            "The secret of getting ahead is getting started. The secret of getting started is breaking your complex overwhelming tasks into small manageable tasks, and then start on the first one.",
            @"Plans are worthless, but planning is everything.
- Dwight Eisenhower",
            @"One of my most productive days was throwing away 1000 lines of code.
- Ken Thompson",
            "Eagleson's Law of Programming: Any code of your own that you haven't looked at for six or more months, might as well have been written by someone else.",
            @"I think it's very important to get more women into computing. My slogan is: Computing is too important to be left to men.
- Karen Spärck Jones",
            @"Java is to JavaScript as ham is to hamster.
- Jeremy Keith",
            @"If you say “I told you so”, you are the one who has failed. Because you knew, but did not manage to stop the train wreck.
- Robert C. Martin",
            @"We crave for new sensations but soon become indifferent to them. Wonders of yesterday are today common occurrences.
- Nikola Tesla",
            @"So much complexity in software comes from trying to make one thing do two things.
- Ryan Singer",
            @"There's nothing more permanent than a temporary hack.
- Kyle Simpson",
            @"It can be better to copy a little code than to pull in a big library for one function. Dependency hygiene trumps code reuse.
- Rob Pyke",
        };

        public TipsWindow()
        {
            InitializeComponent();
            int quoteIndex = new Random().Next(0, quotes.Length);
            messageTextBox.Text = quotes[quoteIndex];
        }

        private void moreButton_Click(object sender, RoutedEventArgs e)
        {
            int quoteIndex = new Random().Next(0, quotes.Length);
            messageTextBox.Text = quotes[quoteIndex];
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
