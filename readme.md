# What is this project for?
Sometimes during meetings with large groups of peoples discussing software development, people can get too far into the technical details of a specific proposed feature, and it's not always clear to the chair of the meeting that this has happened.

The goal of FOB is to allow people in the meeting to let the host know that they think this is happening without derailing the in-progress conversation (they may be wrong, and the discussion could be important).

FOB gives the chair of a meeting a way to see how many people think we should move on, so when enough people have voted to move on, the chair can choose an appropriate time to get the people talking to wrap up the current thread.

# How does it work?
The chair creates a new meeting in FOB, which gives them a meeting ID they can share with each participant so they can "join" the meeting via some device (possibly via  QR code they scan, or maybe even by handing out some preconfigured [AWS buttons](https://aws.amazon.com/iotbutton/))

During the meeting, participants can tap the fob button on their phone/whatever/whatever and the vote count can be seen on some screen that the chair can see (maybe on their laptop which has the meeting agenda). The meeting chair can reset the votes to zero whenever they want (e.g. once the discussion topic has changed)

# How can I run this?
FOB is intended to be running on the open Internet, so that it's easy for participants to join a fob meeting form any device - but there is no freely available server you can use. You'll need to host it yourself - it should work anywhere you can run .NET Core (we use Azure).