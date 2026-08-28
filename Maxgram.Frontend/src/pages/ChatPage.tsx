import { useState, useEffect, useRef } from 'react';
import { useAuth } from '../context/AuthContext';
import { chatsApi, usersApi } from '../api/client';

interface Conversation {
  id: number;
  type: 'Private' | 'Group';
  title: string;
  lastMessage: string | null;
  lastMessageAuthor: string | null;
  lastMessageAt: string | null;
}

interface Message {
  id: number;
  conversationId: number;
  authorId: number;
  authorDisplayName: string;
  text: string | null;
  sentAt: string;
  isDeleted: boolean;
}

export function ChatPage() {
  const { user, logout } = useAuth();
  const [chats, setChats] = useState<Conversation[]>([]);
  const [selectedChat, setSelectedChat] = useState<Conversation | null>(null);
  const [messages, setMessages] = useState<Message[]>([]);
  const [newMessage, setNewMessage] = useState('');
  const [searchQuery, setSearchQuery] = useState('');
  const [searchResults, setSearchResults] = useState<any[]>([]);
  const [showNewChat, setShowNewChat] = useState(false);
  const [showNewGroup, setShowNewGroup] = useState(false);
  const [newGroupTitle, setNewGroupTitle] = useState('');
  const [selectedUsers, setSelectedUsers] = useState<number[]>([]);
  const messagesEndRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    loadChats();
  }, []);

  useEffect(() => {
    if (selectedChat) {
      loadMessages(selectedChat.id);
    }
  }, [selectedChat]);

  useEffect(() => {
    messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
  }, [messages]);

  async function loadChats() {
    try {
      const data = await chatsApi.getAll();
      setChats(data);
    } catch (e) {
      console.error(e);
    }
  }

  async function loadMessages(chatId: number) {
    try {
      const data = await chatsApi.getMessages(chatId);
      setMessages(data.items || []);
    } catch (e) {
      console.error(e);
    }
  }

  async function handleSearch(e: React.FormEvent) {
    e.preventDefault();
    if (!searchQuery.trim()) {
      setSearchResults([]);
      return;
    }
    try {
      const results = await usersApi.search(searchQuery);
      setSearchResults(results.filter((u: any) => u.id !== user?.id));
    } catch (e) {
      console.error(e);
    }
  }

  async function createPrivateChat(userId: number) {
    try {
      const chat = await chatsApi.createPrivate(userId);
      await loadChats();
      setSelectedChat(chat);
      setShowNewChat(false);
      setSearchResults([]);
      setSearchQuery('');
    } catch (e: any) {
      alert(e.message);
    }
  }

  async function createGroupChat() {
    if (!newGroupTitle.trim() || selectedUsers.length === 0) {
      alert('Введите название и выберите участников');
      return;
    }
    try {
      const chat = await chatsApi.createGroup(newGroupTitle, selectedUsers);
      await loadChats();
      setSelectedChat(chat);
      setShowNewGroup(false);
      setNewGroupTitle('');
      setSelectedUsers([]);
    } catch (e: any) {
      alert(e.message);
    }
  }

  async function sendMessage(e: React.FormEvent) {
    e.preventDefault();
    if (!newMessage.trim() || !selectedChat) return;
    try {
      await chatsApi.sendMessage(selectedChat.id, newMessage);
      setNewMessage('');
      loadMessages(selectedChat.id);
      loadChats();
    } catch (e: any) {
      alert(e.message);
    }
  }

  function toggleUserSelection(userId: number) {
    setSelectedUsers(prev =>
      prev.includes(userId)
        ? prev.filter(id => id !== userId)
        : [...prev, userId]
    );
  }

  function formatTime(dateStr: string | null) {
    if (!dateStr) return '';
    const date = new Date(dateStr);
    return date.toLocaleTimeString('ru-RU', { hour: '2-digit', minute: '2-digit' });
  }

  return (
    <div className="chat-layout">
      <aside className="sidebar">
        <div className="sidebar-header">
          <h2>Maxgram</h2>
          <button onClick={logout} className="btn-logout">Выход</button>
        </div>

        <div className="chat-actions">
          <button onClick={() => setShowNewChat(true)}>+ Личный чат</button>
          <button onClick={() => setShowNewGroup(true)}>+ Группа</button>
        </div>

        <div className="chats-list">
          {chats.map(chat => (
            <div
              key={chat.id}
              className={`chat-item ${selectedChat?.id === chat.id ? 'active' : ''}`}
              onClick={() => setSelectedChat(chat)}
            >
              <div className="chat-avatar">{chat.title[0].toUpperCase()}</div>
              <div className="chat-info">
                <div className="chat-title">{chat.title}</div>
                <div className="chat-preview">{chat.lastMessage || 'Нет сообщений'}</div>
              </div>
              <div className="chat-time">{formatTime(chat.lastMessageAt)}</div>
            </div>
          ))}
        </div>
      </aside>

      <main className="chat-main">
        {selectedChat ? (
          <>
            <div className="chat-header">
              <h3>{selectedChat.title}</h3>
            </div>
            <div className="messages">
              {messages.map(msg => (
                <div key={msg.id} className={`message ${msg.authorId === user?.id ? 'own' : ''}`}>
                  <div className="message-author">{msg.authorDisplayName}</div>
                  <div className="message-text">{msg.isDeleted ? '[Сообщение удалено]' : msg.text}</div>
                  <div className="message-time">{formatTime(msg.sentAt)}</div>
                </div>
              ))}
              <div ref={messagesEndRef} />
            </div>
            <form className="message-form" onSubmit={sendMessage}>
              <input
                type="text"
                placeholder="Сообщение..."
                value={newMessage}
                onChange={e => setNewMessage(e.target.value)}
              />
              <button type="submit">Отправить</button>
            </form>
          </>
        ) : (
          <div className="no-chat">Выберите чат</div>
        )}
      </main>

      {showNewChat && (
        <div className="modal">
          <div className="modal-content">
            <h3>Новый чат</h3>
            <form onSubmit={handleSearch}>
              <input
                type="text"
                placeholder="Поиск пользователей..."
                value={searchQuery}
                onChange={e => setSearchQuery(e.target.value)}
              />
              <button type="submit">Найти</button>
            </form>
            <div className="search-results">
              {searchResults.map(u => (
                <div key={u.id} className="search-item" onClick={() => createPrivateChat(u.id)}>
                  <span>{u.displayName}</span>
                  <small>@{u.username}</small>
                </div>
              ))}
            </div>
            <button className="btn-close" onClick={() => setShowNewChat(false)}>Закрыть</button>
          </div>
        </div>
      )}

      {showNewGroup && (
        <div className="modal">
          <div className="modal-content">
            <h3>Новая группа</h3>
            <input
              type="text"
              placeholder="Название группы"
              value={newGroupTitle}
              onChange={e => setNewGroupTitle(e.target.value)}
            />
            <form onSubmit={handleSearch}>
              <input
                type="text"
                placeholder="Добавить участников..."
                value={searchQuery}
                onChange={e => setSearchQuery(e.target.value)}
              />
              <button type="submit">Найти</button>
            </form>
            {selectedUsers.length > 0 && (
              <div className="selected-users">
                Выбрано: {selectedUsers.length}
              </div>
            )}
            <div className="search-results">
              {searchResults.map(u => (
                <div
                  key={u.id}
                  className={`search-item ${selectedUsers.includes(u.id) ? 'selected' : ''}`}
                  onClick={() => toggleUserSelection(u.id)}
                >
                  <span>{u.displayName}</span>
                  <small>@{u.username}</small>
                </div>
              ))}
            </div>
            <button className="btn-create" onClick={createGroupChat}>Создать группу</button>
            <button className="btn-close" onClick={() => setShowNewGroup(false)}>Закрыть</button>
          </div>
        </div>
      )}
    </div>
  );
}