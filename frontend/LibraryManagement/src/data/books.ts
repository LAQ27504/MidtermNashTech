// src/data/books.ts
export type Book = {
    id: string
    title: string
    author: string
    category: string
    available: number
  }
  
  // 10 entries instead of 3
  export const books: Book[] = [
    { id: "1",  title: "1984",              author: "George Orwell",       category: "Fiction",    available: 1 },
    { id: "2",  title: "Clean Code",        author: "Robert C. Martin",    category: "Technology", available: 4 },
    { id: "3",  title: "Sapiens",           author: "Yuval Noah Harari",   category: "History",    available: 3 },
    { id: "4",  title: "The Pragmatic Pro", author: "Andrew Hunt",         category: "Tech",       available: 2 },
    { id: "5",  title: "Dune",              author: "Frank Herbert",       category: "Sci-Fi",     available: 5 },
    { id: "6",  title: "The Hobbit",        author: "J.R.R. Tolkien",      category: "Fantasy",    available: 0 },
    { id: "7",  title: "Leviathan Wakes",   author: "James S. A. Corey",   category: "Sci-Fi",     available: 2 },
    { id: "8",  title: "Eloquent JS",       author: "Marijn Haverbeke",    category: "Programming",available: 7 },
    { id: "9",  title: "You Don’t Know JS", author: "Kyle Simpson",        category: "Programming",available: 3 },
    { id: "10", title: "Design Patterns",   author: "GoF",                 category: "Tech",       available: 4 },
  ]
  