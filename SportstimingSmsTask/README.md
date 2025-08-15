This is a test project that integrates into the some of the API of cpsms. See: https://api.cpsms.dk/documentation/index.html

Class list:
	SportstimingSmsTask - Entry point for sending sms for other systems
	CPSMSSender - Handles sending of sms to cpsms
	SmsSender - Interface for general sms sender
	SendSmsCommandline - Allows sending of sms by commandline using SportstimingSmsTask

CheckCredit() in SportstimingSmsTask returns string because of the format that is returned by cpsms (Like "9.843,40")