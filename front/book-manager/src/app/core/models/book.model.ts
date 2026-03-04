export interface Book {
  id: number;
  title: string;
  isbn: string;
  publishedYear: number;
  description?: string | null;
  authorId: number;
  authorName: string;
  genreId: number;
  genreName: string;
}

export interface CreateBook {
  title: string;
  isbn: string;
  publishedYear: number;
  authorId: number;
  genreId: number;
  description?: string | null;
}

export interface UpdateBook {
  title: string;
  isbn: string;
  publishedYear: number;
  authorId: number;
  genreId: number;
  description?: string | null;
}
