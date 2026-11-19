
import {
  ChevronLeft,
  ChevronRight,
} from "lucide-react";

function Pagination({
  page,
  totalPages,
  totalCount,
  pageSize,
  onPageChange,
  onPageSizeChange,
}) {
  const startItem =
    totalCount === 0 ? 0 : (page - 1) * pageSize + 1;

  const endItem = Math.min(page * pageSize, totalCount);

  return (
    <div className="pagination">
      <div className="page-buttons">
        <button
          type="button"
          disabled={page <= 1}
          onClick={() => onPageChange(page - 1)}
        >
          <ChevronLeft size={17} />
        </button>

        {Array.from(
          { length: totalPages },
          (_, index) => index + 1
        ).map((number) => (
          <button
            type="button"
            key={number}
            className={page === number ? "active" : ""}
            onClick={() => onPageChange(number)}
          >
            {number}
          </button>
        ))}

        <button
          type="button"
          disabled={page >= totalPages || totalPages === 0}
          onClick={() => onPageChange(page + 1)}
        >
          <ChevronRight size={17} />
        </button>
      </div>

      <span className="showing-text">
        Showing {startItem}–{endItem} of {totalCount}
      </span>

      <select
        className="page-size"
        value={pageSize}
        onChange={(e) => onPageSizeChange(Number(e.target.value))}
      >
        <option value={5}>5 per page</option>
        <option value={10}>10 per page</option>
        <option value={25}>25 per page</option>
      </select>
    </div>
  );
}

export default Pagination;
