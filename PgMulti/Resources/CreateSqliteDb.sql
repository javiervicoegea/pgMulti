CREATE TABLE dbs (
    id integer NOT NULL PRIMARY KEY,
    alias character varying(200) NOT NULL UNIQUE,
    server character varying(200) NOT NULL,
    port integer NOT NULL,
    db character varying(200) NOT NULL,
    pass character varying(200) NOT NULL,
    groupid integer NOT NULL,
    position smallint NOT NULL,
    user character varying(200) NOT NULL,
    CONSTRAINT fk_dbs_groups FOREIGN KEY (groupid) REFERENCES groups(id) ON DELETE CASCADE,
    UNIQUE (alias COLLATE NOCASE)
);

CREATE TABLE dbs_logs (
    logid integer NOT NULL,
    dbid integer NOT NULL,
    PRIMARY KEY (logid, dbid),
    CONSTRAINT fk_dbslogs_dbs FOREIGN KEY (dbid) REFERENCES dbs(id) ON DELETE CASCADE,
    CONSTRAINT fk_dbslogs_logs FOREIGN KEY (logid) REFERENCES logs(id) ON DELETE CASCADE
);

CREATE TABLE groups (
    id integer NOT NULL PRIMARY KEY,
    parentgroupid integer NULL,
    name character varying(200) NOT NULL,
    position smallint NOT NULL,
    CONSTRAINT fk_groups_groups FOREIGN KEY (parentgroupid) REFERENCES groups(id) ON DELETE CASCADE
);

CREATE TABLE logs (
    id integer NOT NULL PRIMARY KEY,
    timestamp bigint NOT NULL,
    txt text NOT NULL
);

CREATE TABLE editortabs (
    id integer NOT NULL PRIMARY KEY,
    position smallint NULL,
    text text NOT NULL,
    name text NOT NULL,
    path text NULL,
    closedAt bigint NULL
);

CREATE TABLE config (
    keepServerSelection boolean NOT NULL,
    autocompleteDelay integer NOT NULL,
    mergeTables boolean NOT NULL,
    transactionMode integer NOT NULL,
    transactionLevel integer NOT NULL,
    fontSize integer NOT NULL,
    maxRows integer NOT NULL,
    showWarningSelectedText boolean NOT NULL
);

CREATE INDEX fki_fk_dbs_groups ON dbs(groupid);
CREATE INDEX fki_fk_dbslogs_dbs ON dbs_logs(dbid);
CREATE INDEX fki_fk_dbslogs_logs ON dbs_logs(logid);


INSERT INTO config 
(keepServerSelection,autocompleteDelay,mergeTables,transactionMode,transactionLevel,fontSize,maxRows,showWarningSelectedText) VALUES 
(0,1,1,1,0,10,100,1);

INSERT INTO groups 
(parentgroupid,name,position) VALUES
(null,'root',0);
