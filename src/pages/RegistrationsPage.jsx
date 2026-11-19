
import { useEffect, useState } from "react";

import {
  CarFront,
  Circle,
} from "lucide-react";

import KpiCards from "../components/KpiCards";
import RegistrationFilters from "../components/RegistrationFilters";
import RegistrationTable from "../components/RegistrationTable";
import Pagination from "../components/Pagination";

import {
  getRegistrations,
  getRegistrationSummary,
} from "../api/registrationsApi";

import useDebounce from "../hooks/useDebounce";

function RegistrationsPage() {
  const [registrations, setRegistrations] = useState([]);
  const [summary, setSummary] = useState(null);

  const [participantName, setParticipantName] = useState("");
  const [type, setType] = useState("");
  const [status, setStatus] = useState("");

  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);

  const [totalCount, setTotalCount] = useState(0);
  const [totalPages, setTotalPages] = useState(0);

  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [summaryError, setSummaryError] = useState("");
  const [retryCount, setRetryCount] = useState(0);

  const debouncedParticipantName = useDebounce(
    participantName,
    400
  );

  useEffect(() => {
    let isMounted = true;

    async function fetchSummary() {
      try {
        const data = await getRegistrationSummary();

        if (isMounted) {
          setSummary(data);
          setSummaryError("");
        }
      } catch (err) {
        if (isMounted) {
          setSummaryError(
            err.message || "Failed to load registration summary."
          );
        }
      }
    }

    fetchSummary();

    return () => {
      isMounted = false;
    };
  }, []);

  useEffect(() => {
    let isMounted = true;

    async function fetchRegistrations() {
      try {
        setLoading(true);
        setError("");

        const data = await getRegistrations({
          participantName: debouncedParticipantName,
          type,
          status,
          page,
          pageSize,
        });

        if (!isMounted) return;

        setRegistrations(data.items || []);
        setTotalCount(data.totalCount || 0);
        setTotalPages(data.totalPages || 0);
      } catch (err) {
        if (!isMounted) return;

        setError(
          err.message || "Failed to load registrations."
        );

        setRegistrations([]);
        setTotalCount(0);
        setTotalPages(0);
      } finally {
        if (isMounted) {
          setLoading(false);
        }
      }
    }

    fetchRegistrations();

    return () => {
      isMounted = false;
    };
  }, [
    debouncedParticipantName,
    type,
    status,
    page,
    pageSize,
    retryCount,
  ]);

  const handleParticipantNameChange = (value) => {
    setParticipantName(value);
    setPage(1);
  };

  const handleTypeChange = (value) => {
    setType(value);
    setPage(1);
  };

  const handleStatusChange = (value) => {
    setStatus(value);
    setPage(1);
  };

  const clearFilters = () => {
    setParticipantName("");
    setType("");
    setStatus("");
    setPage(1);
  };

  const handlePageChange = (newPage) => {
    if (newPage >= 1 && newPage <= totalPages) {
      setPage(newPage);
    }
  };

  const handlePageSizeChange = (newPageSize) => {
    setPageSize(newPageSize);
    setPage(1);
  };

  const handleRetry = () => {
    setRetryCount((current) => current + 1);
  };

  return (
    <main className="dashboard">
      {/* HEADER */}
      <header className="top-header">
        <div className="brand">
          <div className="brand-image">
            <CarFront
             size={32}
               strokeWidth={2.8}
              color="#211431"
  />
            </div>

          <div className="brand-details">
            <h1>Car Show 2027</h1>
            <div className="brand-line"></div>
          </div>
        </div>

        <Circle size={22} strokeWidth={1.5} />
      </header>

      {/* KPI SECTION */}
      {summary && <KpiCards summary={summary} />}

      {summaryError && (
        <p className="section-note">{summaryError}</p>
      )}

      {/* REGISTRATIONS SECTION */}
      <section className="registrations-section">
        <div className="table-toolbar">
          <h2>Registrations</h2>

          <RegistrationFilters
            participantName={participantName}
            setParticipantName={handleParticipantNameChange}
            type={type}
            setType={handleTypeChange}
            status={status}
            setStatus={handleStatusChange}
          />
        </div>

        {error ? (
          <div className="empty-message">
            {error}

            <button
              type="button"
              onClick={handleRetry}
            >
              Try Again
            </button>
          </div>
        ) : (
          <>
            <RegistrationTable
              registrations={registrations}
              loading={loading}
              onClearFilters={clearFilters}
            />

            {!loading && (
              <Pagination
                page={page}
                totalPages={totalPages}
                totalCount={totalCount}
                pageSize={pageSize}
                onPageChange={handlePageChange}
                onPageSizeChange={handlePageSizeChange}
              />
            )}
          </>
        )}
      </section>
    </main>
  );
}

export default RegistrationsPage;
