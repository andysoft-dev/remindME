# remindME
Create reminders from linux console that will be sent to your email and whatsapp.

Explanations of projects:

- remindME: Console application
- remindME.Core Core and backend operations
- remindME.Service Brackground service 


Disclaimer: This application use third's components. This components have yours owns licence agreements.

Requirements to use:

- Azure CosmosDB database: required for data persistance
- Sendgrid account: required to send emails.
- CallmeBot API (optional): This web provide an API for send WhatsApp messages (https://www.callmebot.com/blog/free-api-whatsapp-messages/)

Usage:

remindME [options] [arguments]

Options
<ul>
<li>save: Save a reminder</li>
</ul>

Arguments with save option
<ul>
<li>--message "[string]": The message to send</li>
<li>--title "[string]": The title of the message</li>
<li>--time "[datetime]": The datetime when the message will send (format yyyy-MM-dd hh:mm)</li>
<li>--no-whatsapp:  No will send message to WhatsApp</li>
</ul>
