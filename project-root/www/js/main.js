var sock;
var input = document.getElementById('input')
var message = document.getElementById('message')
var button = document.getElementById('send')
var statusMsg = document.getElementById('status')
var app = document.getElementById('app')
var testElem = document.getElementById('test');

var questionIndex = 0

var data = {
  // player's name
  name: 'cool dude',
  // the type of data recieved
  // can be either text or vote
  type: 'text',
  // which story is the player interacting with
  storyId: 0,
  // the text sent to the player
  message: 'here\'s a message!',
  // the options the player is voting between
  voteOptions: [
    'option 1',
    'option 2',
    'option 3'
  ]
}

var ipAddressRegex = /(\d){1,3}.(\d){1,3}.(\d){1,3}.(\d){1,3}/

function init() {
  button.addEventListener('click', sendMessage, false)
}
init()


var serverConnect = function () {
  var element = document.getElementById('server-connect')
  var serverInput = document.getElementById('server-input')
  var nameInput = document.getElementById('name-input')
  var connectButton = document.getElementById('connect-button')

  function toggleElement(force) {
    element.classList.toggle('hidden', !force)
  }

  function clearIpInput() {
    serverInput.value = ''
  }

  function username() {
    return nameInput.value
  }

  function init() {
    serverInput.addEventListener('keydown', connectServer, false)
    connectButton.addEventListener('click', connectServer, false)
    console.log('server init')
  }
  init()

  function connectServer(e) {
    if (e.keyCode === 13 || e.keyCode === undefined) {
      var server = serverInput.value
      if (server.match(ipAddressRegex)) {
        setStatus('trying to connect')
        sock = new WebSocket('ws://' + server + ':9999')
        sock.addEventListener('open', connect, false)
        sock.addEventListener('message', onMessage, false)
        sock.addEventListener('error', onError, false)
        sock.addEventListener('close', onClose, false)
      } else {
        setStatus('incorrect format for ip address!')
      }
    }
  }

  return {
    toggleElement: toggleElement,
    clearIpInput: clearIpInput,
    username: username
  }
}()

var voting = function () {
  element = document.getElementById('voting')

  function displayVoteOptions(options) {
    removeAllChildren(element)
    var ul = document.createElement('ul')
    for (var i = 0; i < options.length; i++) {
      var li = document.createElement('li')
      var btn = document.createElement('button')
      btn.textContent = options[i]
      btn.setAttribute('id', options[i])
      btn.addEventListener('click', vote, false)
      li.appendChild(btn)
      ul.appendChild(li)
    }
    element.appendChild(ul)
  }

  function vote(e) {
    console.log(e)
  }

  return {
    displayVoteOptions: displayVoteOptions
  }
}()

function setStatus(text) {
  console.log(text)
  statusMsg.textContent = text
}

function connect() {
  serverConnect.toggleElement(false)
  data.name = serverConnect.username()

  _data = JSON.stringify(data)
  sock.send(_data)
  console.log('sent ↓')
  console.log(_data)

  testElem.classList.remove('hidden')
}

function onMessage(e) {
  data = JSON.parse(e.data)

  if (data.type === 'text') {
    message.textContent = data.message
  } else if (data.type === 'vote') {
    voting.displayVoteOptions(data.voteOptions)
  }

  console.log('got ↓')
  console.log(data)
}

function sendMessage() {
  data.answers[questionIndex] = input.value
  sock.send(data)
}

function onError(e) {
  setStatus('there was an issue connecting with this server, double check your connection and ip address');
}

function onClose() {
  setStatus('connection closed')
  serverConnect.toggleElement(true)
  serverConnect.clearIpInput()
}

function removeAllChildren(parent) {
  while (parent.firstChild) {
    parent.removeChild(parent.firstChild);
  }
}

